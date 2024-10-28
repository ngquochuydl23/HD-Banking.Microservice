#!/bin/sh

echo "Waiting for Kafka to be ready..."
# Lặp lại đến khi Kafka sẵn sàng
until kafka-topics --bootstrap-server server-1.pgonevn.com:9092 --list; do
    sleep 5
done

echo "Creating Kafka topics..."
kafka-topics --bootstrap-server server-1.pgonevn.com:9092 --create --if-not-exists --topic my-topic-1 --replication-factor 1 --partitions 1
kafka-topics --bootstrap-server server-1.pgonevn.com:9092 --create --if-not-exists --topic my-topic-2 --replication-factor 1 --partitions 1

echo "Kafka topics created successfully!"
kafka-topics --bootstrap-server server-1.pgonevn.com:9092 --list
