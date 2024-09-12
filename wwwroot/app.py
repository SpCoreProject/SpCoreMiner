from flask import Flask, request, jsonify
import subprocess

app = Flask(__name__)

@app.route('/temperature', methods=['GET'])
def get_temperature():
    try:
        temp = subprocess.check_output(["vcgencmd", "measure_temp"]).decode("utf-8")
        temp = float(temp.replace("temp=", "").replace("'C\n", ""))
        return jsonify({"temperature": temp})
    except Exception as e:
        return jsonify({"error": str(e)})

@app.route('/set_mode', methods=['POST'])
def set_mode():
    mode = request.form.get('mode')
    if mode in ['default', 'turbo', 'echo', 'off']:
        # Call the appropriate Python script or function to change fan mode
        # Example: subprocess.call(["python3", "fan_control.py", mode])
        return jsonify({"status": "success"})
    else:
        return jsonify({"error": "Invalid mode"})

if __name__ == "__main__":
    app.run(host='0.0.0.0', port=5000)
