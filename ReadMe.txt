services.msc




















node --max-old-space-size=4096 main.js


c:\log_11111\redis-server.exe --service-install --service-name log_11111 --port 11111
npm install redis

c:\jpol_pawn_care_20002\redis-server.exe --service-install --service-name jpol_pawn_care_20002 --port 20002
c:\jpol_pawn_chiatay_20001\redis-server.exe --service-install --service-name jpol_pawn_chiatay_20001 --port 20001
c:\jpol_pawn_addnew_edit_20000\redis-server.exe --service-install --service-name jpol_pawn_addnew_edit_20000 --port 20000

node --max-old-space-size=4096 api_test.js

load ./txt-full/POL_PROCESS.txt
load ./txt/POL_PROCESS.txt

c:\db_sync_19999\redis-server.exe --service-install --service-name db_sync_19999 --port 19999

c:\pol_log_10000\redis-server.exe --service-install --service-name pol_log_10000 --port 10000

c:\pos_user_10001\redis-server.exe --service-install --service-name pos_user_10001 --port 10001
c:\pos_shop_10002\redis-server.exe --service-install --service-name pos_shop_10002 --port 10002
c:\pos_group_10003\redis-server.exe --service-install --service-name pos_group_10003 --port 10003
c:\pol_sys_sms_10004\redis-server.exe --service-install --service-name pol_sys_sms_10004 --port 10004
c:\pol_sys_email_10005\redis-server.exe --service-install --service-name pol_sys_email_10005 --port 10005
c:\pol_support_schedule_10006\redis-server.exe --service-install --service-name pol_support_schedule_10006 --port 10006
c:\pol_step_10007\redis-server.exe --service-install --service-name pol_step_10007 --port 10007
c:\pol_reason_fail_10008\redis-server.exe --service-install --service-name pol_reason_fail_10008 --port 10008
c:\pol_process_10009\redis-server.exe --service-install --service-name pol_process_10009 --port 10009
c:\pol_priority_10010\redis-server.exe --service-install --service-name pol_priority_10010 --port 10010
c:\pol_pawn_search_10011\redis-server.exe --service-install --service-name pol_pawn_search_10011 --port 10011
c:\pol_pawn_detail_10012\redis-server.exe --service-install --service-name pol_pawn_detail_10012 --port 10012
c:\pol_pawn_10013\redis-server.exe --service-install --service-name pol_pawn_10013 --port 10013
c:\pol_notify_10014\redis-server.exe --service-install --service-name pol_notify_10014 --port 10014
c:\pol_customer_10015\redis-server.exe --service-install --service-name pol_customer_10015 --port 10015
c:\pol_channel_10016\redis-server.exe --service-install --service-name pol_channel_10016 --port 10016
c:\pol_asset_type_10017\redis-server.exe --service-install --service-name pol_asset_type_10017 --port 10017
c:\pol_area_10018\redis-server.exe --service-install --service-name pol_area_10018 --port 10018
c:\pos_sys_config_10019\redis-server.exe --service-install --service-name pos_sys_config_10019 --port 10019

.\redis-server.exe --service-install --service-name pol_log --port 10000
sc delete pol_log

npm install mssql node-fetch cron uuid node-fetch express body-parser csv-stringify lodash tedious redis socket.io performance-now response-time kue -S
npm install cookie-parser grpc redis level grpc @grpc/proto-loader socket.io -S





npm install node-fetch cron uuid node-fetch express body-parser lodash -S

redis-server.exe -k install -n "Redis 16000" --port 16000 --service-install redis.windows.conf --loglevel verbose
redis-server.exe --port 11000 --service-install redis.windows.conf --loglevel verbose

node --max-old-space-size=4096 core.js

npm install mustache node-fetch cron uuid node-fetch express body-parser csv-stringify mustache lodash tedious -S

npm install cookie-parser grpc redis level grpc @grpc/proto-loader socket.io -S

npm install kue
npm install response-time

.\redis-server.exe — service-install --port 19000 redis.windows.conf — loglevel verbose




npm install cron redis




services.msc
RUN > Certmgr.msc



httpd.exe -k install -n "Apache HTTP Server"
nginx.exe -k install -n "NGINX HTTP Server"

nginx.exe -s start
nginx.exe -s stop

TASKKILL /F /IM "nginx*"

TASKKILL /F /IM "httpd*"

Update-Package -reinstall


 Windows+R, then type shell:startup.


 start.bat

@ECHO OFF
REM Start Nginx
tasklist /FI "IMAGENAME eq nginx.exe" 2>NUL | find /I /N "nginx.exe">NUL
IF NOT "%ERRORLEVEL%"=="0" (
   REM Nginx is NOT running, so start it
   c:
   cd \nginx
   start nginx.exe
   ECHO Nginx started.
) else (
   ECHO Nginx is already running.
)

stop.bat

@ECHO OFF
REM Stop Nginx
tasklist /FI "IMAGENAME eq nginx.exe" 2>NUL | find /I /N "nginx.exe">NUL
IF "%ERRORLEVEL%"=="0" (
   REM Nginx is currently running, so quit it
   c:
   cd \nginx
   nginx.exe -s quit
   ECHO Nginx quit issued.
) else (
   ECHO Nginx is not currently running.
)













