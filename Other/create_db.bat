echo off

rem batch file to run a script to create a db
rem 2019-09-05

sqlcmd -S localhost -E -i EZDealer_db.sql
rem -S localhost\mssqlserver -E -i EZDealer_db.sql
rem -S localhost\sqlexpress -E -i EZDealer_db.sql

ECHO .
ECHO If no error messages appear DB was created
PAUSE