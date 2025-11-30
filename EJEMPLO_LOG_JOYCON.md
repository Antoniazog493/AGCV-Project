# ?? EJEMPLO DE LOG - Joy-Con Event Monitor

## Sesión de ejemplo con ambos Joy-Cons

```
[14:23:45.123] [SYSTEM] ?? AGCV Button Tester iniciado
[14:23:45.125] [SYSTEM] Esperando input del Joy-Con...
[14:23:45.234] [SYSTEM] Controlador encontrado en slot 1

# Usuario presiona botón A del Joy-Con derecho
[14:23:47.234] [INPUT] [Joy-Con R] A            PRESSED   @ 2109ms
[14:23:47.456] [INPUT] [Joy-Con R] A            RELEASED  @ 2331ms | Duration: 222ms

# Usuario presiona botón B
[14:23:48.123] [INPUT] [Joy-Con R] B            PRESSED   @ 2998ms
[14:23:48.289] [INPUT] [Joy-Con R] B            RELEASED  @ 3164ms | Duration: 166ms

# Usuario usa D-Pad izquierdo
[14:23:49.456] [INPUT] [Joy-Con L] D-PAD ?      PRESSED   @ 4331ms
[14:23:49.678] [INPUT] [Joy-Con L] D-PAD ?      RELEASED  @ 4553ms | Duration: 222ms

[14:23:50.123] [INPUT] [Joy-Con L] D-PAD ?      PRESSED   @ 4998ms
[14:23:50.345] [INPUT] [Joy-Con L] D-PAD ?      RELEASED  @ 5220ms | Duration: 222ms

# Usuario presiona gatillo derecho
[14:23:51.567] [TRIGGER] [Joy-Con R] RT           VALUE: 128/255 (50%) @ 6442ms
[14:23:51.789] [TRIGGER] [Joy-Con R] RT           VALUE: 255/255 (100%) @ 6664ms

# Usuario presiona Start
[14:23:52.123] [INPUT] [Joy-Con R] START (+)    PRESSED   @ 6998ms
[14:23:52.234] [INPUT] [Joy-Con R] START (+)    RELEASED  @ 7109ms | Duration: 111ms

# Usuario presiona múltiples botones simultáneamente
[14:23:53.456] [INPUT] [Joy-Con R] A            PRESSED   @ 8331ms
[14:23:53.467] [INPUT] [Joy-Con R] B            PRESSED   @ 8342ms
[14:23:53.678] [INPUT] [Joy-Con R] A            RELEASED  @ 8553ms | Duration: 222ms
[14:23:53.689] [INPUT] [Joy-Con R] B            RELEASED  @ 8564ms | Duration: 222ms

# Usuario solicita información del controlador
[14:23:55.123] [INFO] ???????????????????????????????????????????????????
[14:23:55.124] [INFO] Información del Controlador #1
[14:23:55.125] [INFO] ???????????????????????????????????????????????????
[14:23:55.126] [INFO] Tipo de batería: Ni-MH
[14:23:55.127] [INFO] Nivel de batería: Alta (75%)
[14:23:55.128] [INFO] Latencia promedio: ~8ms
[14:23:55.129] [INFO] Frecuencia de polling: ~120Hz
[14:23:55.130] [INFO] Tiempo activo: 00:00:10
[14:23:55.131] [INFO] Total de eventos: 12
[14:23:55.132] [INFO] ???????????????????????????????????????????????????

# Usuario presiona shoulder buttons
[14:23:56.456] [INPUT] [Joy-Con L] LB           PRESSED   @ 11331ms
[14:23:56.678] [INPUT] [Joy-Con L] LB           RELEASED  @ 11553ms | Duration: 222ms

[14:23:57.123] [INPUT] [Joy-Con R] RB           PRESSED   @ 11998ms
[14:23:57.345] [INPUT] [Joy-Con R] RB           RELEASED  @ 12220ms | Duration: 222ms

# Usuario hace un combo rápido (D-Pad arriba + A)
[14:23:58.456] [INPUT] [Joy-Con L] D-PAD ?      PRESSED   @ 13331ms
[14:23:58.467] [INPUT] [Joy-Con R] A            PRESSED   @ 13342ms
[14:23:58.556] [INPUT] [Joy-Con L] D-PAD ?      RELEASED  @ 13431ms | Duration: 100ms
[14:23:58.567] [INPUT] [Joy-Con R] A            RELEASED  @ 13442ms | Duration: 100ms

# Usuario presiona Back (-)
[14:23:59.789] [INPUT] [Joy-Con L] BACK (-)     PRESSED   @ 14664ms
[14:23:59.890] [INPUT] [Joy-Con L] BACK (-)     RELEASED  @ 14765ms | Duration: 101ms

# Usuario hace click en los sticks
[14:24:01.123] [INPUT] [Joy-Con L] L-STICK      PRESSED   @ 15998ms
[14:24:01.234] [INPUT] [Joy-Con L] L-STICK      RELEASED  @ 16109ms | Duration: 111ms

[14:24:02.456] [INPUT] [Joy-Con R] R-STICK      PRESSED   @ 17331ms
[14:24:02.567] [INPUT] [Joy-Con R] R-STICK      RELEASED  @ 17442ms | Duration: 111ms

# Ejemplo de PROBLEMA DETECTADO: Botón pegado
[14:24:05.123] [INPUT] [Joy-Con R] Y            PRESSED   @ 19998ms
[14:24:05.345] [INPUT] [Joy-Con R] Y            RELEASED  @ 20220ms | Duration: 222ms
[14:24:05.356] [INPUT] [Joy-Con R] Y            PRESSED   @ 20231ms  ?? Rebote!
[14:24:05.367] [INPUT] [Joy-Con R] Y            RELEASED  @ 20242ms | Duration: 11ms
[14:24:05.378] [INPUT] [Joy-Con R] Y            PRESSED   @ 20253ms  ?? Rebote!
[14:24:05.389] [INPUT] [Joy-Con R] Y            RELEASED  @ 20264ms | Duration: 11ms

# Usuario limpia el log
[14:24:10.123] [SYSTEM] Log limpiado

# Usuario cierra el monitor
[14:24:15.456] [SYSTEM] Button Tester cerrado
```

---

## ?? ANÁLISIS DEL LOG

### ? **Comportamiento Normal:**
```
[14:23:47.234] [INPUT] [Joy-Con R] A PRESSED @ 2109ms
[14:23:47.456] [INPUT] [Joy-Con R] A RELEASED @ 2331ms | Duration: 222ms
```
- **Duración:** 222ms (normal para pulsación humana)
- **Sin rebotes**
- **Timing consistente**

### ?? **Problema Detectado:**
```
[14:24:05.123] [INPUT] [Joy-Con R] Y PRESSED @ 19998ms
[14:24:05.345] [INPUT] [Joy-Con R] Y RELEASED @ 20220ms | Duration: 222ms
[14:24:05.356] [INPUT] [Joy-Con R] Y PRESSED @ 20231ms  ??
[14:24:05.367] [INPUT] [Joy-Con R] Y RELEASED @ 20242ms | Duration: 11ms
```
- **Múltiples PRESSED/RELEASED** seguidos
- **Duraciones muy cortas** (11ms)
- **Indica botón pegado o falso contacto**

---

## ?? ESTADÍSTICAS DE LA SESIÓN

- **Duración total:** ~30 segundos
- **Eventos registrados:** ~30+
- **Joy-Con L eventos:** ~12
- **Joy-Con R eventos:** ~18
- **Problemas detectados:** 1 (botón Y con rebote)

---

## ?? FORMATO DE EXPORTACIÓN

Este log puede guardarse como archivo `.txt` o `.log` usando el botón **?? Export**.

**Nombre de archivo sugerido:**
```
JoyCon_Log_20250120_142345.txt
```

---

## ?? USOS PRÁCTICOS

1. **Diagnóstico:** Identificar botones defectuosos
2. **Soporte Técnico:** Enviar log a soporte
3. **Análisis:** Revisar patrones de uso
4. **Documentación:** Evidencia de problemas
5. **Testing:** Verificar reparaciones

---

**Ejemplo generado por:** AGCV Joy-Con Event Monitor v2.0
