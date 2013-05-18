<?php

/**
 * Tera Launcher
 * Database Configuration
 * Do not delete this file as it connects
 * to many other services and apis required
 * by the TeraEMU system.
 */
 
$DBHost = 'localhost';
$DBUser = 'root';
$DBPass = '';
$DBName = 'dbtera';
$DBPort = 3306;

// Check if PDO is Installed!
if( !defined('PDO::ATTR_DRIVER_NAME'))
{
	exit("Tera API Requires PDO!, Please update PHP!");
}

// Connect using PDO!
$dbh = new PDO('mysql:host='.$DBHost.';port='.$DBPort.';dbname='.$DBName,
$DBUser, $DBPass, array( PDO::ATTR_PERSISTENT => false ) );