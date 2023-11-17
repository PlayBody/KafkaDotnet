# Project: Kafka Consumer and Rest API Forwarder

## Description
This project is a simple .NET Core application which listens to a Kafka topic and forwards any data found on the topic to a REST endpoint.

## Requirements
* .NET Core
* Kafka
* Python (For the test HTTP server)

## Solution
I developed it on windows 11.

### Program.cs
This is the main entry point of the application. It sets up a host for the application to run. It includes the following key components:

* Configuration: It loads app settings from a Json file (appsettings.json).
* Services: It adds necessary services to the dependency injection container, including an HTTP client, the KafkaConsumerService, and configurations for Kafka and the API.
* Logging: It sets up console logging.
### KafkaConsumerService.cs
This is a background service that continuously listens to a Kafka topic and forwards the messages to a REST endpoint.

* Constructor: It initializes the Kafka consumer, HTTP client, logger, and the REST endpoint URL.
* ExecuteAsync: This method is executed when the service starts. It subscribes to the Kafka topic and continuously consumes messages until the service is stopped. It forwards each consumed message to the REST endpoint and logs the response status.
* Dispose: This method is called when the service is stopped. It unsubscribes from the topic, closes and disposes of the Kafka consumer.
### ApiSettings.cs
This class represents the API settings read from the configuration file. It includes the URL of the REST endpoint to which the messages are forwarded.

### appsettings.json
This file contains the application's settings, including Kafka consumer settings like the bootstrap server and group ID, and the API settings like the REST endpoint URL.

### test_http.py
This is a simple Flask application that serves as the REST endpoint for testing purposes. It listens to POST requests at the /kafka endpoint and just echoes the message received in the request.


## Architecture
The application is built employing good design practices, keeping scalability, extensibility and performance in mind. It uses the Confluent.Kafka library to consume data from a Kafka topic and push it to a REST endpoint.

The application is designed to plug in any REST endpoint URL and push data to that endpoint. It also uses a BackgroundService to run the Kafka consumer in the background.

## Setup

### Start Zookeeper:
in kafka installed directory.
```shell
.\bin\windows\zookeeper-server-start.bat .\config\zookeeper.properties
```
### Start Kafka:

```shell
.\bin\windows\kafka-server-start.bat .\config\server.properties
```

### Create a Kafka topic:
```
.\bin\windows\kafka-topics.bat --create --topic myAwesomeTopic --bootstrap-server localhost:9092 --partitions 1 --replication-factor 1
```

### Flask Test HTTP Server
To test the functionality, we can use the provided Flask server (test_http.py). 
```python
python test_http.py
```

## Running the Application

### Configuration
The application's configuration can be found in the appsettings.json file. Here, we can specify the Kafka server and group ID details under Kafka, and the REST endpoint URL under ApiSettings.

### Build and Run
Once we've set up the configuration, we can build and run the application.

### Testing the Application
we can test the application by sending a message to the Kafka topic:
```
.\bin\windows\kafka-console-producer.bat --broker-list localhost:9092 --topic myAwesomeTopic
```
This will prompt we to input a message. Once we send a message, the application will consume it from the Kafka topic and forward it to the REST endpoint.

## Design Decisions and Code Quality
The application is built with clean code and good design patterns. The design allows for scalability and extensibility. we can add more consumers or producers, depending on the application's requirements.

Each class has a specific responsibility, keeping the code modular and easy to maintain. The BackgroundService ensures that the Kafka consumer keeps running in the background and consumes data as and when it arrives on the topic.

Error handling is in place to handle any exceptions during consumption and forwarding of data. The application logs detailed information about the exceptions.

## Future Enhancements
The application can be extended to include more features, such as:

* Error retries
* Support for multiple Kafka topics
* Support for different types of data and serialization formats
* Support for secure communication with Kafka and the REST endpoint