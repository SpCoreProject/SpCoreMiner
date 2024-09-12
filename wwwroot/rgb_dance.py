import RPi.GPIO as GPIO
import time
import signal
import sys

# تنظیم پین‌ها
GPIO.setmode(GPIO.BCM)
GPIO.setup(4, GPIO.OUT, initial=GPIO.LOW)  # قرمز
GPIO.setup(22, GPIO.OUT, initial=GPIO.LOW)  # سبز
GPIO.setup(27, GPIO.OUT, initial=GPIO.LOW)  # آبی

# تنظیم PWM برای هر رنگ
red = GPIO.PWM(17, 100)  # 100 هرتز برای PWM
green = GPIO.PWM(22, 100)
blue = GPIO.PWM(27, 100)

# شروع PWM با مقدار اولیه 0 (لامپ خاموش)
red.start(0)
green.start(0)
blue.start(0)

def fade_in_out(color, start, end, step, delay):
    for i in range(start, end, step):
        color.ChangeDutyCycle(i)
        time.sleep(delay)

def signal_handler(sig, frame):
    # خاموش کردن چراغ‌ها و پاکسازی GPIO در هنگام دریافت سیگنال
    red.stop()
    green.stop()
    blue.stop()
    GPIO.cleanup()
    sys.exit(0)

# متصل کردن سیگنال SIGTERM و SIGINT به signal_handler
signal.signal(signal.SIGTERM, signal_handler)
signal.signal(signal.SIGINT, signal_handler)

try:
    while True:
        # افزایش قرمز و کاهش سبز و آبی
        fade_in_out(red, 0, 101, 1, 0.02)
        fade_in_out(green, 100, -1, -1, 0.02)
        fade_in_out(blue, 100, -1, -1, 0.02)

        # افزایش سبز و کاهش قرمز و آبی
        fade_in_out(red, 100, -1, -1, 0.02)
        fade_in_out(green, 0, 101, 1, 0.02)
        fade_in_out(blue, 100, -1, -1, 0.02)

        # افزایش آبی و کاهش قرمز و سبز
        fade_in_out(red, 100, -1, -1, 0.02)
        fade_in_out(green, 100, -1, -1, 0.02)
        fade_in_out(blue, 0, 101, 1, 0.02)

except KeyboardInterrupt:
    pass

finally:
    # پاکسازی نهایی
    red.stop()
    green.stop()
    blue.stop()
    GPIO.cleanup()
