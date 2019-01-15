#!/usr/bin/env bash

name='symtestX'$1
log_name='kill'$name
docker kill $name > $log_name