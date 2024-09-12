import RPi.GPIO as GPIO
import time
import threading
import os

# پین‌های مربوط به فن‌ها
FAN1_PIN = 12  # تغییر به پین مناسب
FAN2_PIN = 16  # تغییر به پین مناسب

# تنظیمات سرعت فن
DEFAULT_SPEED = 50    # سرعت پیش‌فرض (درصد)
TURBO_SPEED = 75      # سرعت توربو (درصد)
ECHO_SPEED = 25       # سرعت اکو (درصد)

# تنظیمات دما
DEFAULT_TEMP_THRESHOLD = 60  # آستانه دما برای تغییر سرعت فن

# تنظیمات GPIO
GPIO.setmode(GPIO.BCM)
GPIO.setup(FAN1_PIN, GPIO.OUT)
GPIO.setup(FAN2_PIN, GPIO.OUT)

fan1_pwm = GPIO.PWM(FAN1_PIN, 100)  # فرکانس PWM
fan2_pwm = GPIO.PWM(FAN2_PIN, 100)  # فرکانس PWM

fan1_pwm.start(DEFAULT_SPEED)
fan2_pwm.start(DEFAULT_SPEED)

def get_cpu_temperature():
    """گرفتن دمای CPU"""
    temp = os.popen("vcgencmd measure_temp").readline()
    return float(temp.replace("temp=", "").replace("'C\n", ""))

def set_fan_speed(speed):
    """تنظیم سرعت فن‌ها"""
    fan1_pwm.ChangeDutyCycle(speed)
    fan2_pwm.ChangeDutyCycle(speed)

def adjust_fan_speed():
    """تنظیم سرعت فن‌ها بر اساس دما"""
    while True:
        temp = get_cpu_temperature()
        if temp > DEFAULT_TEMP_THRESHOLD:
            set_fan_speed(TURBO_SPEED)
        else:
            set_fan_speed(DEFAULT_SPEED)
        time.sleep(10)  # هر 10 ثانیه بررسی کنید

def change_fan_mode(mode):
    """تغییر حالت فن"""
    if mode == "turbo":
        set_fan_speed(TURBO_SPEED)
    elif mode == "echo":
        set_fan_speed(ECHO_SPEED)
    else:
        set_fan_speed(DEFAULT_SPEED)

def start_service():
    """شروع سرویس"""
    threading.Thread(target=adjust_fan_speed, daemon=True).start()

if __name__ == "__main__":
    start_service()
    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        GPIO.cleanup()
