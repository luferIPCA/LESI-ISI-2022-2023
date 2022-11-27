

O topic do MQTT broker tem de ser gerado no kettle...

Exemplo:

Se o topico gerado no Kettle for "sensor"

O broker tem de subscrever esse topico:

mosquitto_sub -t sensor -h localhost