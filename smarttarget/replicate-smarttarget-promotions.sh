#!/bin/bash

# Copyright (c) 2016 SDL Group. 
#
# Licensed under the Apache License, Version 2.0 (the "License"); you may not 
# use this file except in compliance with the License. You may obtain a copy 
# of the License at: http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software distributed 
# under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
# CONDITIONS OF ANY KIND, either express or implied. See the License for the 
# specific language governing permissions and limitations under the License.



# =============================================================================== 
# REPLICATE SMARTTARGET PROMOTIONS
#
# Description:  This script replicates SmartTarget promotions between two 
#               Fredhopper instances by copying the business.xml configuration 
#               file from this instance to the other (remote) Fredhopper.
#
#
# Dependencies: scp                         used for copying business.xml 
#               curl                        used for reloading configuration
#               customer-post-publish       used for invoking this script
#
#
# Remarks:      This script is invoked by the bash script registered with
#               Fredhopper as the customer-post-publish script.
#               https://www.fredhopper.com/learningcenter/x/zEEbAQ
#
#               For the customer-post-publish script to be called, ensure
#               that ApproveEveryChange is set to true in smarttarget_conf.xml       
#
#               The customer-post-publish script sets the parameters for
#               this script.
#
#               This script assumes that the supplied user account has been
#               configured for password-less authentication on ssh/scp using
#               public keys.
#               https://simonljb123.wordpress.com/2013/07/17/how-to-use-sshscp-without-password-on-centos-6-4/
#
#
#
# ===============================================================================

DEFAULT_HTTP_PORT="8180"
DEFAULT_BASEPATH="/fredhopper"
DEFAULT_CATALOG="catalog01"

usage() {

  echo
  echo REPLICATE SMARTTARGET PROMOTIONS
  echo
  echo "Usage:" 
  echo  
  echo "    replicate-smarttarget-promotions.sh --target=[host] --httpport=[8180]" 
  echo "       --user=[uid] --basepath=[/fredhopper] --catalog=[catalog01]"
  echo
  echo
  echo "Parameters:"
  echo
  echo "  -t=, --target=       hostname of the remote Fredhopper host"
  echo
  echo "  -p=, --httpport=     http port of the remote Fredhopper instance"
  echo "                       default: ${DEFAULT_HTTP_PORT}"
  echo
  echo "  -u=, --user=         username to use for ssh/scp of business.xml file"
  echo
  echo "  -b=, --basepath=     install location of Fredhopper"
  echo "                       default: ${DEFAULT_BASEPATH}"
  echo
  echo "  -c=, --catalog=      Fredhopper catalog used for SmartTarget"
  echo "                       default: ${DEFAULT_CATALOG}"
  echo

  exit 1

}

die() {
  echo >&2 "$@"
  exit 2
}

# -------------------------------------------------------------------------------
# PARSE COMMAND LINE ARGUMENTS
# based on http://stackoverflow.com/questions/192249/how-do-i-parse-command-line-arguments-in-bash
# -------------------------------------------------------------------------------

if [ $# -eq 0 ] 
  then 
    usage
fi


for i in "$@"
do
  case $i in
    -t=*|--target=*)
      TARGET="${i#*=}"
      shift
      ;;
    -p=*|--httpport=*)
      HTTP_PORT="${i#*=}"
      shift
      ;;
    -u=*|--user=*)
      REMOTE_USER="${i#*=}"
      shift
      ;;
    -b=*|--basepath=*)
      BASEPATH="${i#*=}"
      shift
      ;;
    -c=*|--catalog=*)
      CATALOG="${i#*=}"
      shift
      ;;
    *)
      ;;
esac
done

# ---------------------------------------------------
# mandatory parameters
# ---------------------------------------------------
if [ -z "${TARGET}"  ] 
  then 
    die "Target hostname not set." 
fi

if [ -z "${REMOTE_USER}" ]
  then
    die "SCP username not set."
fi

# ---------------------------------------------------
# parameters with default values
# ---------------------------------------------------
if [ -z "${HTTP_PORT}" ]
  then
    HTTP_PORT="${DEFAULT_HTTP_PORT}"
fi
if [ -z "${BASEPATH}" ]
  then
    BASEPATH="${DEFAULT_BASEPATH}"
fi
if [ -z "${CATALOG}" ]
  then
    CATALOG="${DEFAULT_CATALOG}"
fi 



# -----------------------------------------------------------------------------
# GENERATE COMMAND VARIABLES
# -----------------------------------------------------------------------------
# path of the business.xml file just published by local Fredhopper instance
PUBLISHED_BUSINESS_XML="${BASEPATH}/data/instances/${CATALOG}/data/business/business.xml"

# path of the business.xml file currently used by local Fredhopper instance
ACTIVE_BUSINESS_XML="${BASEPATH}/data/instances/${CATALOG}/config/business.xml"

# scp target path for published business.xml on remote Fredhopper instance
TARGET_PUBLISHED_BUSINESS_XML="${REMOTE_USER}@${TARGET}:${PUBLISHED_BUSINESS_XML}"

# scp target path for current business.xml on remote Fredhopper instance
TARGET_ACTIVE_BUSINESS_XML="${REMOTE_USER}@${TARGET}:${ACTIVE_BUSINESS_XML}"

# remote Fredhopper URL to reload business.xml configuration
TARGET_RELOAD_URL="http://${TARGET}:${HTTP_PORT}/fredhopper/sysadmin/reload-config.jsp?select=business"


# ----------------------------------------------------------------------------
# EXECUTION
# ----------------------------------------------------------------------------

# copy published business.xml to remote instance
scp ${PUBLISHED_BUSINESS_XML} ${TARGET_PUBLISHED_BUSINESS_XML}

# copy active business.xml to remote instance
scp ${ACTIVE_BUSINESS_XML} ${TARGET_ACTIVE_BUSINESS_XML}

# reload business.xml configuration on remote instance
curl ${TARGET_RELOAD_URL}



# ============================================================================
# END
# ============================================================================

