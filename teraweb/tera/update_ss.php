<?php
	
	// Check for Empty $_GET
	if( empty( $_GET ) )
	{
		die("Empty GET!");
	}
	
	// Server List File
	$serfile = 'launcher/servers/serlist.en';
	
	// Verification
	$authorized = array( '127.0.0.1', '10.0.0.4', '25.149.74.211', '::1' );
	if( !in_array( $_SERVER['REMOTE_ADDR'], $authorized ) ){ return false; }
	
	include_once("db.php");
	
	// XML Layout
	$xml = <<<EOF
<?xml version="1.0" encoding="utf-8"?>
\n
EOF;

	$xml .= <<<EOF
<serverlist>
\n
EOF;
	
	switch( $_GET['status'] )
	{
		case "0":
		$sth = $dbh->prepare("SELECT * 
		FROM `servers` 
		WHERE name = ?");
		$sth->bindValue(1, $_GET['server']);
		$sth->execute();
		
		$result = $sth->fetch();
		$result[9] = ( ( $result[9] == 1 ) ? 'Open' : 'Closed' );
		
		$sth = $dbh->prepare("UPDATE `servers` 
		SET l_visible = 1 
		WHERE name = ?");
		$sth->bindValue(1, $_GET['server']);
		$sth->execute();
		
		$xml .=<<<EOF
  <server>
    <id>$result[0]</id>
	<ip>$result[4]</ip>
	<port>$result[5]</port>
	<category>$result[6]</category>
	<name raw_name="$result[1]">
	  <![CDATA[ $result[1] ]]>
	</name>
	<crowdness sort="1">$result[8]</crowdness>
	<open sort="1">$result[9]</open>
	<permission_mask>$result[10]</permission_mask>
	<server_stat>$result[11]</server_stat>
	<popup>
	  <![CDATA[ Play Now. ]]>
	</popup>
	<language>$result[12]</language>
  </server>
\n
EOF;
	$xml .= <<<EOF
</serverlist>
EOF;
		unlink($serfile);
		$handle = fopen($serfile, 'a+');
		fwrite($handle, $xml);
		fclose($handle);
		
		
			break;
		case "offline":
		
		$sth = $dbh->prepare("UPDATE `servers` 
		SET l_visible = 0 
		WHERE name = ?");
		$sth->bindValue(1, $_GET['server']);
		$sth->execute();
		
		unlink($serfile);
			break;
	}

	
?>