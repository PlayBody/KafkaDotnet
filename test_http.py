from flask import Flask, request

app = Flask(__name__)

@app.route('/kafka', methods=['POST'])
def kafka_handler():
  json = request.get_json()
  return json["message"]

if __name__ == '__main__':
  app.run(port=8000)