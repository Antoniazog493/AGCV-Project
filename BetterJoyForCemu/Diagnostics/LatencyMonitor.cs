using System;
using System.Diagnostics;

namespace BetterJoyForCemu.Diagnostics {
    /// <summary>
    /// Measures and tracks input latency metrics for Joy-Cons with separate tracking for left/right
    /// </summary>
    public class LatencyMonitor {
        private readonly Stopwatch _timer = Stopwatch.StartNew();
        private long _lastUpdateTime = 0;
        private long _minLatency = long.MaxValue;
        private long _maxLatency = 0;
        private long _totalLatency = 0;
        private int _sampleCount = 0;
        
        // Separate tracking for left Joy-Con (usually has more latency)
        private long _leftMinLatency = long.MaxValue;
        private long _leftMaxLatency = 0;
        private long _leftTotalLatency = 0;
        private int _leftSampleCount = 0;
        
        // Right Joy-Con tracking
        private long _rightMinLatency = long.MaxValue;
        private long _rightMaxLatency = 0;
        private long _rightTotalLatency = 0;
        private int _rightSampleCount = 0;
        
        private const int MAX_SAMPLES = 1000;
        
        public void RecordUpdate(bool isLeft = false) {
            long currentTime = _timer.ElapsedTicks;
            
            if (_lastUpdateTime > 0) {
                long latency = currentTime - _lastUpdateTime;
                long latencyMs = (latency * 1000) / Stopwatch.Frequency;
                
                // Overall stats
                _minLatency = Math.Min(_minLatency, latencyMs);
                _maxLatency = Math.Max(_maxLatency, latencyMs);
                _totalLatency += latencyMs;
                _sampleCount++;
                
                // Per-controller stats
                if (isLeft) {
                    _leftMinLatency = Math.Min(_leftMinLatency, latencyMs);
                    _leftMaxLatency = Math.Max(_leftMaxLatency, latencyMs);
                    _leftTotalLatency += latencyMs;
                    _leftSampleCount++;
                } else {
                    _rightMinLatency = Math.Min(_rightMinLatency, latencyMs);
                    _rightMaxLatency = Math.Max(_rightMaxLatency, latencyMs);
                    _rightTotalLatency += latencyMs;
                    _rightSampleCount++;
                }
                
                // Reset stats after MAX_SAMPLES to keep them current
                if (_sampleCount >= MAX_SAMPLES) {
                    Reset();
                }
            }
            
            _lastUpdateTime = currentTime;
        }
        
        public double GetAverageLatencyMs() {
            return _sampleCount > 0 ? (double)_totalLatency / _sampleCount : 0;
        }
        
        public double GetLeftAverageLatencyMs() {
            return _leftSampleCount > 0 ? (double)_leftTotalLatency / _leftSampleCount : 0;
        }
        
        public double GetRightAverageLatencyMs() {
            return _rightSampleCount > 0 ? (double)_rightTotalLatency / _rightSampleCount : 0;
        }
        
        public long GetMinLatencyMs() => _minLatency == long.MaxValue ? 0 : _minLatency;
        public long GetMaxLatencyMs() => _maxLatency;
        public int GetSampleCount() => _sampleCount;
        
        public long GetLeftMinLatencyMs() => _leftMinLatency == long.MaxValue ? 0 : _leftMinLatency;
        public long GetLeftMaxLatencyMs() => _leftMaxLatency;
        
        public long GetRightMinLatencyMs() => _rightMinLatency == long.MaxValue ? 0 : _rightMinLatency;
        public long GetRightMaxLatencyMs() => _rightMaxLatency;
        
        public void Reset() {
            _minLatency = long.MaxValue;
            _maxLatency = 0;
            _totalLatency = 0;
            _sampleCount = 0;
            
            _leftMinLatency = long.MaxValue;
            _leftMaxLatency = 0;
            _leftTotalLatency = 0;
            _leftSampleCount = 0;
            
            _rightMinLatency = long.MaxValue;
            _rightMaxLatency = 0;
            _rightTotalLatency = 0;
            _rightSampleCount = 0;
        }
        
        public string GetStats() {
            if (_sampleCount == 0) return "No data";
            
            return $"Latency - Avg: {GetAverageLatencyMs():F2}ms, Min: {GetMinLatencyMs()}ms, Max: {GetMaxLatencyMs()}ms (n={_sampleCount})";
        }
        
        public string GetDetailedStats() {
            if (_sampleCount == 0) return "No data";
            
            string overall = $"Overall - Avg: {GetAverageLatencyMs():F2}ms, Min: {GetMinLatencyMs()}ms, Max: {GetMaxLatencyMs()}ms";
            string left = _leftSampleCount > 0 ? 
                $"\nLeft    - Avg: {GetLeftAverageLatencyMs():F2}ms, Min: {GetLeftMinLatencyMs()}ms, Max: {GetLeftMaxLatencyMs()}ms (n={_leftSampleCount})" : "";
            string right = _rightSampleCount > 0 ? 
                $"\nRight   - Avg: {GetRightAverageLatencyMs():F2}ms, Min: {GetRightMinLatencyMs()}ms, Max: {GetRightMaxLatencyMs()}ms (n={_rightSampleCount})" : "";
            
            return overall + left + right;
        }
    }
}
