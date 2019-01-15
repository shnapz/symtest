#!/usr/bin/env bash

name='symtestX'$1
port1='808'$1
port2='500'$1
docker run --name $name --rm -it -p $port1:$port2 symtest > $name
cp ~/projects/symtest/symtest.Tests.Integration/teardown_wrapper.sh ~/projects/symtest/symtest.Tests.Integration/bin/Debug/netcoreapp2.2/teardown_wrapper.sh