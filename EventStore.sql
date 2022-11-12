
CREATE DATABASE sae_dev IF NOT EXISTS;

use sae_dev;

create table if not exists `event_stream`(
id varchar(128) not null,
timestamp datetime not null,
version bigint not null,
data longtext
)ENGINE=InnoDB DEFAULT CHARSET=utf8;


create table if not exists `snapshot`(
id varchar(128) not null,
version bigint not null,
data longtext not null
)engine=InnoDB default charset=utf8;