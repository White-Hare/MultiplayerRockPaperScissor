<?php
$connect = new mysqli("localhost", "root", "", "rock_paper_scissor");

if($connect->error)
    die("An Error Occurred While Connecting To Database");

