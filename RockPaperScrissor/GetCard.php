<?php
require "ConnectToServer.php";

#$enemy = $_POST["player2name"];
$enemy = "METE";

$result = $connect->query("SELECT card FROM player WHERE playername = '$enemy'");
if($result && $result->num_rows > 0)
    echo $result->fetch_assoc()["card"];
else
    die("Player(\"$enemy\") Not Found");