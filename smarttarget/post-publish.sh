#!/bin/sh

# execute promotion replication script
/fredhopper/catalog01/bin/replicate-smarttarget-promotions.sh --target=sdlfh-live-2.hyper-v.local --httpport=8180 --username=root --basepath=/fredhopper --catalog=smarttarget

# remove lock file
if [ -f "/fredhopper/tmp/publish.lck" ]; then
  rm /fredhopper/tmp/publish.lck
else
  echo "Publisher lock file not found."
fi
