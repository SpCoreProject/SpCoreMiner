import os
import time
import subprocess

def get_ip_address():
    try:
        # Get the IP address assigned to wlan0
        result = subprocess.check_output(["hostname", "-I"]).decode().strip()
        return result
    except Exception as e:
        return f"Error retrieving IP address: {str(e)}"

def connect_to_wifi():
    try:
        # Restart the wlan0 interface
        os.system("sudo ifdown wlan0")
        time.sleep(2)
        os.system("sudo ifup wlan0")
        
        time.sleep(5)  # Wait a few seconds to ensure the connection is established
        ip_address = get_ip_address()
        
        if ip_address:
            return f"Connected. IP Address: {ip_address}"
        else:
            return "Connected, but failed to retrieve IP address."
    except Exception as e:
        return f"Failed to reconnect: {str(e)}"

if __name__ == "__main__":
    print(connect_to_wifi())
