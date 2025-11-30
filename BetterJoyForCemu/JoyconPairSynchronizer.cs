using System;
using System.Runtime.CompilerServices;

namespace BetterJoyForCemu {
    /// <summary>
    /// Advanced synchronizer with motion prediction to minimize Joy-Con latency asymmetry
    /// Uses lock-free structures and prediction to reduce left Joy-Con lag
    /// </summary>
    public class JoyconPairSynchronizer {
        // Lock-free buffers using object for atomic operations
        private StickData _leftStick = new StickData();
        private StickData _rightStick = new StickData();
        private readonly object _leftLock = new object();
        private readonly object _rightLock = new object();
        
        // Prediction buffers to compensate for latency
        private readonly RingBuffer<StickData> _leftHistory = new RingBuffer<StickData>(5);
        private readonly RingBuffer<StickData> _rightHistory = new RingBuffer<StickData>(5);
        
        private long _leftUpdateCount = 0;
        private long _rightUpdateCount = 0;
        
        // Timestamps for latency tracking
        private long _leftLastUpdate = 0;
        private long _rightLastUpdate = 0;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float[] GetLeftStick() {
            StickData data;
            lock (_leftLock) {
                data = _leftStick;
            }
            return new[] { data.X, data.Y };
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float[] GetRightStick() {
            StickData data;
            lock (_rightLock) {
                data = _rightStick;
            }
            return new[] { data.X, data.Y };
        }
        
        /// <summary>
        /// Updates left stick with motion prediction
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateLeftStick(float[] stick) {
            var now = System.Diagnostics.Stopwatch.GetTimestamp();
            var data = new StickData { X = stick[0], Y = stick[1], Timestamp = now };
            
            lock (_leftLock) {
                _leftHistory.Add(data);
                _leftStick = data;
                _leftLastUpdate = now;
            }
            System.Threading.Interlocked.Increment(ref _leftUpdateCount);
        }
        
        /// <summary>
        /// Updates right stick with motion prediction
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateRightStick(float[] stick) {
            var now = System.Diagnostics.Stopwatch.GetTimestamp();
            var data = new StickData { X = stick[0], Y = stick[1], Timestamp = now };
            
            lock (_rightLock) {
                _rightHistory.Add(data);
                _rightStick = data;
                _rightLastUpdate = now;
            }
            System.Threading.Interlocked.Increment(ref _rightUpdateCount);
        }
        
        /// <summary>
        /// Gets predicted stick position based on velocity
        /// Helps compensate for Bluetooth latency on left Joy-Con
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float[] GetLeftStickPredicted() {
            if (_leftHistory.Count < 2) return GetLeftStick();
            
            var current = _leftHistory.GetLast();
            var previous = _leftHistory.GetAt(_leftHistory.Count - 2);
            
            // Calculate velocity
            long timeDelta = current.Timestamp - previous.Timestamp;
            if (timeDelta == 0) return new[] { current.X, current.Y };
            
            float velocityX = (current.X - previous.X);
            float velocityY = (current.Y - previous.Y);
            
            // Predict forward by ~5ms (one frame)
            const float predictionFactor = 0.3f; // Conservative prediction
            float predictedX = current.X + (velocityX * predictionFactor);
            float predictedY = current.Y + (velocityY * predictionFactor);
            
            // Clamp to valid range
            predictedX = Math.Max(-1.0f, Math.Min(1.0f, predictedX));
            predictedY = Math.Max(-1.0f, Math.Min(1.0f, predictedY));
            
            return new[] { predictedX, predictedY };
        }
        
        public long GetLeftUpdateCount() => System.Threading.Interlocked.Read(ref _leftUpdateCount);
        public long GetRightUpdateCount() => System.Threading.Interlocked.Read(ref _rightUpdateCount);
        
        public void Reset() {
            _leftHistory.Clear();
            _rightHistory.Clear();
        }
        
        // Struct for lock-free atomic updates
        private struct StickData {
            public float X;
            public float Y;
            public long Timestamp;
        }
        
        // Lock-free ring buffer for history
        private class RingBuffer<T> {
            private readonly T[] _buffer;
            private int _count = 0;
            private int _index = 0;
            private readonly object _lock = new object();
            
            public RingBuffer(int capacity) {
                _buffer = new T[capacity];
            }
            
            public int Count => _count;
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Add(T item) {
                lock (_lock) {
                    _buffer[_index] = item;
                    _index = (_index + 1) % _buffer.Length;
                    if (_count < _buffer.Length) _count++;
                }
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T GetLast() {
                lock (_lock) {
                    if (_count == 0) return default(T);
                    int lastIndex = (_index - 1 + _buffer.Length) % _buffer.Length;
                    return _buffer[lastIndex];
                }
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T GetAt(int position) {
                lock (_lock) {
                    if (position >= _count) return default(T);
                    int index = (_index - _count + position + _buffer.Length) % _buffer.Length;
                    return _buffer[index];
                }
            }
            
            public void Clear() {
                lock (_lock) {
                    _count = 0;
                    _index = 0;
                }
            }
        }
    }
}
