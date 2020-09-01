<?php
require "ConnectToServer.php";

$player = $_POST["playername"];
$card = $_POST["card"];

$result = $connect->query("SELECT * FROM player WHERE playername = '$player'");
 if($result == false || $result->num_rows == 0)
    die("Player(\"$player\") Not Found");

$connect->query("UPDATE player SET card = '$card' WHERE playername = '$player'");

if($result == false)
    echo "An Error Occurred\n";

$connect->close();