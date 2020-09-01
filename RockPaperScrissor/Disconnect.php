<?php
require "ConnectToServer.php";

$roomid  = $_POST["roomid"];


$result = $connect->query("SELECT player1name, player2name FROM room WHERE roomid = '$roomid'");
if($result && $result->num_rows > 0) {
    $result = $result->fetch_assoc();
    $updater = $connect->prepare("UPDATE player SET card = NULL WHERE playername = ? OR playername = ?");
    $updater->bind_param("ss", $result["player1name"], $result["player2name"]);
    $updater->execute();
}

$del = $connect->prepare("DELETE FROM room WHERE ?");
$del->bind_param("s", $r);

$r = $roomid;
$del->execute();


$del->close();
$connect->close();