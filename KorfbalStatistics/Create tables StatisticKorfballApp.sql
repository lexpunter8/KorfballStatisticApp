use StatisticKorfballApp;

DROP table IF EXISTS Attack_Shot;
DROP table IF EXISTS Attack_Rebound;
DROP table IF EXISTS Attack;
DROP table IF EXISTS Attack_Goal;
DROP table IF EXISTS GoalType;
DROP table IF EXISTS Formation;
DROP table IF EXISTS Game;
DROP table IF EXISTS Player;
DROP table IF EXISTS Coach;
DROP table IF EXISTS Team;
DROP table IF EXISTS User;

CREATE TABLE `User` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Sex` char(1),
  `Username` varchar(100),
  `Password` varchar(100),
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `Team` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `Player` (
  `Id` int(11) NOT NULL,
  `Team_Id` int(11),
  `Number` int(11),
  PRIMARY KEY (`Id`,`Team_Id`),
  KEY `FK_Team_Id` (`Team_Id`),
  CONSTRAINT `Player_ibfk_1` FOREIGN KEY (`Team_Id`) REFERENCES `Team` (`Id`),
  CONSTRAINT `Player_ibfk_2` FOREIGN KEY (`Id`) REFERENCES `User` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `Coach` (
  `User_Id` int(11) NOT NULL,
  `Team_Id` int(11) NOT NULL,
  PRIMARY KEY (`User_Id`,`Team_Id`),
  CONSTRAINT FOREIGN KEY (`Team_Id`) REFERENCES `Team` (`Id`),
  CONSTRAINT FOREIGN KEY (`User_Id`) REFERENCES `User` (`Id`)
);

CREATE TABLE `Game` (
  `Id` int(11) NOT NULL auto_increment,
  `Team_Id` int(11) NOT NULL,
  `Opponent` varchar(100) NOT NULL,
  `Date` datetime NOT NULL,  
  `IsHome` tinyint NOT NULL,  
  PRIMARY KEY (`Id`),
  CONSTRAINT FOREIGN KEY (`Team_Id`) REFERENCES `Team` (`Id`)
);

CREATE TABLE `Formation` (
  `Id` int(11) NOT NULL auto_increment,
  `Player_Id` int(11) NOT NULL,
  `Game_Id` int(11) NOT NULL,
  `Function` varchar(1) NOT NULL,  
  PRIMARY KEY (`Id`),
  CONSTRAINT FOREIGN KEY (`Player_Id`) REFERENCES `Player` (`Id`),
  CONSTRAINT FOREIGN KEY (`Game_Id`) REFERENCES `Game` (`Id`)
);

CREATE TABLE `GoalType` (
  `Id` int(11) NOT NULL auto_increment,
  `Name` varchar(100) NOT NULL, 
  PRIMARY KEY (`Id`)
);

CREATE TABLE `Attack_Goal` (
  `Id` int(11) NOT NULL,
  `GoalType_Id` int(11) NOT NULL,
  `Player_Id` int(11) NOT NULL,  
  PRIMARY KEY (`Id`),
  CONSTRAINT FOREIGN KEY (`Player_Id`) REFERENCES `Player` (`Id`),
  CONSTRAINT FOREIGN KEY (`GoalType_Id`) REFERENCES `GoalType` (`Id`)
);

CREATE TABLE `Attack` (
  `Id` int(11) NOT NULL auto_increment,
  `Goal_Id` int(11),
  `Turnover_Player_Id` int(11),
  `Game_Id` int(11) NOT NULL,
  `IsShotClockOverride` tinyint NOT NULL,  
  `IsAttackFunction` tinyint NOT NULL,  
  `IsDefence` tinyint NOT NULL,  
  PRIMARY KEY (`Id`),
  CONSTRAINT FOREIGN KEY (`Goal_Id`) REFERENCES `Attack_Goal` (`Id`),
  CONSTRAINT FOREIGN KEY (`Game_Id`) REFERENCES `Game` (`Id`),
  CONSTRAINT FOREIGN KEY (`Turnover_Player_Id`) REFERENCES `Player` (`Id`)
);

CREATE TABLE `Attack_Rebound` (
  `Attack_Id` int(11) NOT NULL,
  `Player_Id` int(11) NOT NULL,  
  `Count` int(11) NOT NULL,  
  PRIMARY KEY (`Attack_Id`, `Player_Id`),
  CONSTRAINT FOREIGN KEY (`Player_Id`) REFERENCES `Player` (`Id`),
  CONSTRAINT FOREIGN KEY (`Attack_Id`) REFERENCES `Attack` (`Id`)
);

CREATE TABLE `Attack_Shot` (
  `Attack_Id` int(11) NOT NULL,
  `Player_Id` int(11) NOT NULL,  
  `Count` int(11) NOT NULL,  
  PRIMARY KEY (`Attack_Id`, `Player_Id`),
  CONSTRAINT FOREIGN KEY (`Player_Id`) REFERENCES `Player` (`Id`),
  CONSTRAINT FOREIGN KEY (`Attack_Id`) REFERENCES `Attack` (`Id`)
);

