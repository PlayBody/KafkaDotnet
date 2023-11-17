zookeeper:
	.\bin\windows\zookeeper-server-start.bat .\config\zookeeper.properties

kafka-start:
	.\bin\windows\kafka-server-start.bat .\config\server.properties

topic-enroll:
	.\bin\windows\kafka-topics.bat --create --topic myAwesomeTopic --bootstrap-server localhost:9092 --partitions 1 --replication-factor 1

topic-message:
	.\bin\windows\kafka-console-producer.bat --broker-list localhost:9092 --topic myAwesomeTopic