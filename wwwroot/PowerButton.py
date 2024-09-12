import RPi.GPIO as GPIO
import time
import os

# تعریف پین‌ها برای هر دکمه
button_pin_1 = 3  # سمت راست
button_pin_2 = 2  # سمت چپ

# تنظیم GPIO
GPIO.setmode(GPIO.BCM)
GPIO.setup(button_pin_1, GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.setup(button_pin_2, GPIO.IN, pull_up_down=GPIO.PUD_UP)

def handle_button_1(channel):
    # دکمه سمت راست فشرده شده است
    print("Button 1 pressed!")
    # اینجا می‌توانید عملکرد مورد نظر برای دکمه سمت راست را قرار دهید
    os.system("sudo reboot")  # مثال: ری‌استارت کردن سیستم

def handle_button_2(channel):
    # دکمه سمت چپ فشرده شده است
    print("Button 2 pressed!")
    # اینجا می‌توانید عملکرد مورد نظر برای دکمه سمت چپ را قرار دهید
    os.system("sudo shutdown -h now")  # مثال: خاموش کردن سیستم

# تنظیم رویدادها برای هر دکمه
GPIO.add_event_detect(button_pin_1, GPIO.FALLING, callback=handle_button_1, bouncetime=200)
GPIO.add_event_detect(button_pin_2, GPIO.FALLING, callback=handle_button_2, bouncetime=200)

try:
    while True:
        time.sleep(1)
except KeyboardInterrupt:
    GPIO.cleanup()
