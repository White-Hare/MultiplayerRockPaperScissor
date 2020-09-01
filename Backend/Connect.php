<?php
require "ConnectToServer.php";

$player = $_POST["playername"];

$selectquery = $connect->prepare("SELECT id FROM player WHERE playername = ?");
if($selectquery == FALSE)
    var_dump($connect->error);

$selectquery->bind_param("s",$value);

$value = $player;
$selectquery->execute();
$result = $selectquery->get_result();
if($result == false || $result->num_rows == 0)
    $connect->query("INSERT INTO player(`playername`) VALUES ('$player')");

$result = $connect->query("SELECT * FROM room WHERE player2name IS NULL");
if($result == null || $result == false || $result->num_rows == 0)
    $connect->query("INSERT INTO room(player1name) VALUES ('$player')");

else {
    $roomid = $result->fetch_assoc()["roomid"];
    $connect->query("UPDATE room SET player2name = '$player' WHERE roomid = '$roomid'");
}

$result = $connect->query("SELECT * FROM room WHERE `player1name` = '$player' OR `player2name` = '$player'");
echo json_encode($result->fetch_assoc());

$selectquery->close();
$connect->close();
