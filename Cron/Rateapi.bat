@echo off
cd "C:\inetpub\wwwroot\gefx\Cron"
wget -O "reminder_records.log" http://10.100.3.5/rateapi/getrate