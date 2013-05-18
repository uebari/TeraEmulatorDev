<?php

error_reporting(0);

// Set Headers for Output Data
header('Content-Type: text/html; charset=utf-8');

// Database Connection Information
require_once('db.php');

$Action = $_GET['action'];
if( isset($Action) && $Action <> "" )
{
	switch( $Action )
	{
		case "login":
			Login();
			break;
		case "register":
			Register();
			break;
		case "serverlist":
			break;
	}
}

function ErrDisplay( $string, $errCode = 0, $actionCode = null )
{
	$jsonTemplate = "{\"success\":\"false\",\"code\":\"$errCode\",\"actionCode\":\"$actionCode\",\"error\":\"$string\"}";
	exit( $jsonTemplate );
}

function textParse()
{
	foreach( $_GET as $k => $v )
	{
		$data[strtolower($k)] = strtolower($v);
	}
	return $data;
}

function CheckUser( $username )
{
	global $dbh;
	
	$userData = textParse();
	
	// Search Username
	$sth = $dbh->prepare("SELECT COUNT(*) FROM `accounts`
	WHERE name = ?");
	$sth->bindValue(1, $userData['username'], PDO::PARAM_STR);
	
	$sth->execute();
	$count = $sth->Fetch()[0];
	
	if( $count >= 1 )
		ErrDisplay( "Username already exists", 4, $userData['action'] );
}

function Register()
{
	global $dbh;
	
	$userData = textParse();
	
	if( $userData['username'] == null ) 
	{ 
		ErrDisplay( "Username is empty!", 5, $userData['action'] );
	}
	if( $userData['password'] == null || $userData['rpassword'] == null )
	{
		ErrDisplay("Password is empty!", 6, $userData['action']);
	}
	if( strlen( $userData['username'] ) > 15 )
	{
		ErrDisplay("Username can not be more than 15 characters", 9, $userData['action']);
	}
	if( strlen( $userData['password'] ) > 15 || strlen( $userData['password'] ) < 6 )
	{
		ErrDisplay("Password is more than 15 characters or less than 6 characters.", 10, $userData['action'] );
	}
	if( $userData['password'] <> $userData['rpassword'] )
	{
		ErrDisplay("Password do not match!", 15, $userData['action']);
	}
	if( !filter_var($userData['email'], FILTER_VALIDATE_EMAIL) )
	{
		ErrDisplay("E-Mail is not valid!", 20, $userData['action']);
	}
	
	// Do we exist already?
	checkUser( $userData['username'] );
	
	$password = md5( $userData['username'] . $userData['password'] );
	
	$sth = $dbh->prepare("INSERT INTO `accounts` (name, password)
			VALUES(?, ?);
		   ");
	$sth->bindValue(1, $userData['username'], PDO::PARAM_STR);
	$sth->bindValue(2, $password, PDO::PARAM_STR);
	$sth->execute();
	
	Login();
}

function Login()
{
	global $dbh;
	
	$userData = textParse();
	
	if( $userData['username'] == null || $userData['password'] == null )
	{
		ErrDisplay( "Fields are empty!", 1, $userData['action'] );
	}
	
	// Password
	$password = md5( $userData['username'] . $userData['password'] );
	$sth = $dbh->prepare("SELECT * FROM `accounts`
	WHERE name = ?
	AND password = ?");
	$sth->bindValue(1, $userData['username'], PDO::PARAM_STR);
	$sth->bindValue(2, $password, PDO::PARAM_STR);
	$sth->execute();
	
	$u = $sth->fetch();
	if( empty( $u ) )
	{
		ErrDisplay( "User does not Exist or password incorrect!", 2, $userData['action'] );
	}
	
	// Run an Update!
	/*$sth = $dbh->prepare("UPDATE `accounts` SET LastOnlineUtc = CURRENT_TIMESTAMP(),
	lastip = ? WHERE name = ? AND password = ?");
	$sth->bindValue(1, $_SERVER['REMOTE_ADDR']);
	$sth->bindValue(2, $userData['username'], PDO::PARAM_STR);
	$sth->bindValue(3, $password, PDO::PARAM_STR);
	$sth->execute();*/
	
	//print_r($u);
	
	$jsonTemplate = "{\"success\":\"true\",
	\"username\":\"".$u['name']."\",
	\"password\":\"".$u['password']."\",
	\"id\":\"".$u['id']."\",
	\"coins\":\"".$u['coins']."\"}";
	
	echo $jsonTemplate;
	
}