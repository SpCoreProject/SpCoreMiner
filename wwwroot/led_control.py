import sys
import RPi.GPIO as GPIO

# تنظیم پین‌های GPIO برای LEDها
led_pins = {
    "cpu_red": 15,
    "cpu_green": 14,
    "cart_red": 1,
    "cart_green": 23,
    "echo_red": 0,
    "echo_green": 21,
    "echo_yellow": 5,
    "logo_white": 6,
    "connection_red": 13,
    "connection_green": 19
}

GPIO.setmode(GPIO.BCM)

# تنظیم پین‌ها به‌عنوان خروجی
for pin in led_pins.values():
    GPIO.setup(pin, GPIO.OUT)

# تابع برای خاموش کردن تمامی LEDهای یک دسته
def turn_off_leds(option):
    red_pin = led_pins.get(f"{option}_red")
    green_pin = led_pins.get(f"{option}_green")
    white_pin = led_pins.get(f"{option}_white")
    yellow_pin = led_pins.get(f"{option}_yellow")

    if red_pin is not None:
        GPIO.output(red_pin, GPIO.LOW)
    if green_pin is not None:
        GPIO.output(green_pin, GPIO.LOW)
    if white_pin is not None:
        GPIO.output(white_pin, GPIO.LOW)
    if yellow_pin is not None:
        GPIO.output(yellow_pin, GPIO.LOW)

# تابع برای کنترل LED
def control_led(option, status):
    turn_off_leds(option)
    
    red_pin = led_pins.get(f"{option}_red")
    green_pin = led_pins.get(f"{option}_green")
    white_pin = led_pins.get(f"{option}_white")
    yellow_pin = led_pins.get(f"{option}_yellow")

    if status == "red_on" and red_pin is not None:
        GPIO.output(red_pin, GPIO.HIGH)
    elif status == "green_on" and green_pin is not None:
        GPIO.output(green_pin, GPIO.HIGH)
    elif status == "yellow_on" and yellow_pin is not None:
        GPIO.output(yellow_pin, GPIO.HIGH)
    elif status == "white_on" and white_pin is not None:
        GPIO.output(white_pin, GPIO.HIGH)

# دریافت ورودی‌ها از خط فرمان
if len(sys.argv) == 3:
    option = sys.argv[1]
    status = sys.argv[2]
    control_led(option, status)
else:
    print("Invalid arguments")
