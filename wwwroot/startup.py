import RPi.GPIO as GPIO
import time
 
GPIO.setmode(GPIO.BCM)
 
led_pins = [14, 23, 5, 21, 15, 1, 0, 6, 13]

for pin in led_pins:
    GPIO.setup(pin, GPIO.OUT)
     
fan1_pin = 12  
fan2_pin = 16  

GPIO.setup(fan1_pin, GPIO.OUT)
GPIO.setup(fan2_pin, GPIO.OUT)
 
GPIO.output(18, GPIO.LOW)
GPIO.output(23, GPIO.LOW)
GPIO.output(5, GPIO.LOW)
GPIO.output(21, GPIO.LOW)
 
GPIO.output(15, GPIO.HIGH)
GPIO.output(1, GPIO.HIGH)
GPIO.output(0, GPIO.HIGH)
GPIO.output(6, GPIO.HIGH)
GPIO.output(13, GPIO.HIGH)
 
 
#fan1_pwm = GPIO.PWM(fan1_pin, 100)  
#fan2_pwm = GPIO.PWM(fan2_pin, 100)  
 
#fan1_pwm.start(50)
#fan2_pwm.start(50) 

GPIO.output(fan1_pin, GPIO.HIGH)
GPIO.output(fan2_pin, GPIO.HIGH)
#chmod +x /var/www/html/Main/wwwroot/startup.py
#crontab -e
#@reboot /var/www/html/Main/wwwroot/startup.py

