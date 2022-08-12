CREATE DATABASE SAE_DEV;

use SAE_DEV;

create table if not exists EventStream(
id varchar(128) not null,
timestamp datetime not null,
version bigint not null,
data longtext
)ENGINE=InnoDB DEFAULT CHARSET=utf8;


create table if not exists Snapshot(
id varchar(128) not null,
type varchar(128) not null,
version bigint not null,
data longtext not null
)engine=InnoDB default charset=utf8;


