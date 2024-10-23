redis-cli default ip is localhost/127.0.0.1 and port is 6379

on windows can install with chocolatey and then install redis
or
use docker to get redis image. ex: docker run -p 6380:6379 --name some-redis -d redis
and get can check with docker exec -it "container id" sh then connect redis-cli to test with ping

Test docker ps if container runs

For GUI is popular and free to use Another Redis desktop manager https://github.com/qishibo/AnotherRedisDesktopManager/


Redis Data Types :
String 
- SET "key" "value"
- GET "key"
- GETRANGE "key" 0 -1
- INCR "key"
- DCR "key"
- INCRBY "key" number


List
-LPUSH/RPUSH "key" "value"
-LRANGE "key" 0 -1
-LPOP/RPOP deletes first data on direction
-LINDEX "key" indexno

Set
-SADD "key" "value"
-SMEMBERS "key"
-SREM "key" "value"

Sorted set
-ZADD "key" [scoreNo] "value"
-ZRANGE "key" 0 -1 [extra WITHSCORES]
-ZREM "key" "value"

Hash
-HMSET "hashname" "key" "pair"
-HGET "hashname" "key"
-HREM "hashname" "key"
-HGETALL "hashname"