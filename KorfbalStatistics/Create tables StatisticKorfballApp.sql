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
  `Id` varchar(36) NOT NULL,
  `ClusteredIndex` int NOT NULL auto_increment,
  `Name` varchar(100) NOT NULL,
  `First_Name` varchar(100) NOT NULL,
  `Sex` char(1),
  `Username` varchar(100),
  `Password` varchar(100),
  PRIMARY KEY (`Id`),
  unique key (`ClusteredIndex`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `Team` (
  `Id` varchar(36) NOT NULL,
  `ClusteredIndex` int NOT NULL auto_increment,
  `Name` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`),
  unique key (`ClusteredIndex`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `Player` (
  `Id` varchar(36) NOT NULL,
  `Team_Id` varchar(36),
  `Number` int(11),
  `Abbrevation` varchar(5),
  PRIMARY KEY (`Id`,`Team_Id`),
  CONSTRAINT `Player_ibfk_1` FOREIGN KEY (`Team_Id`) REFERENCES `Team` (`Id`),
  CONSTRAINT `Player_ibfk_2` FOREIGN KEY (`Id`) REFERENCES `User` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `Coach` (
  `User_Id` varchar(36) NOT NULL,
  `Team_Id` varchar(36) NOT NULL,
  PRIMARY KEY (`User_Id`,`Team_Id`),
  CONSTRAINT FOREIGN KEY (`Team_Id`) REFERENCES `Team` (`Id`),
  CONSTRAINT FOREIGN KEY (`User_Id`) REFERENCES `User` (`Id`)
);

CREATE TABLE `Game` (
  `Id` varchar(36) NOT NULL,
  `ClusteredIndex` int NOT NULL auto_increment,
  `Team_Id` varchar(36) NOT NULL,
  `Opponent` varchar(100) NOT NULL,
  `Date` datetime NOT NULL,  
  `IsHome` tinyint NOT NULL,  
  `Status` varchar(5) NOT NULL,
  PRIMARY KEY (`Id`),
  unique key (`ClusteredIndex`),
  CONSTRAINT FOREIGN KEY (`Team_Id`) REFERENCES `Team` (`Id`)
);

CREATE TABLE `Formation` (
  `Id` varchar(36) NOT NULL,
  `ClusteredIndex` int NOT NULL auto_increment,
  `Player_Id` varchar(36) NOT NULL,
  `Game_Id` varchar(36) NOT NULL,
  `CurrentFunction` varchar(1) NOT NULL,  
  `StartFunction` varchar(1) NOT NULL,  
  PRIMARY KEY (`Id`),
  unique key (`ClusteredIndex`),
  CONSTRAINT FOREIGN KEY (`Player_Id`) REFERENCES `Player` (`Id`),
  CONSTRAINT FOREIGN KEY (`Game_Id`) REFERENCES `Game` (`Id`)
);

CREATE TABLE `GoalType` (
  `Id` varchar(36) NOT NULL,
  `ClusteredIndex` int NOT NULL auto_increment,
  `Name` varchar(100) NOT NULL, 
  PRIMARY KEY (`Id`),
  unique key (`ClusteredIndex`)
);

CREATE TABLE `Attack_Goal` (
  `Id` varchar(36) NOT NULL,
  `ClusteredIndex` int NOT NULL auto_increment,
  `GoalType_Id` varchar(36) NOT NULL,
  `Player_Id` varchar(36) NOT NULL,  
  `Assist_Player_Id` varchar(36) NOT NULL,  
  PRIMARY KEY (`Id`),
  unique key (`ClusteredIndex`),
  CONSTRAINT FOREIGN KEY (`Player_Id`) REFERENCES `Player` (`Id`),
  CONSTRAINT FOREIGN KEY (`Assist_Player_Id`) REFERENCES `Player` (`Id`),
  CONSTRAINT FOREIGN KEY (`GoalType_Id`) REFERENCES `GoalType` (`Id`)
);
CREATE TABLE `Attack` (
  `Id` varchar(36) NOT NULL,
  `ClusteredIndex` int NOT NULL auto_increment,
  `Goal_Id` varchar(36),
  `Turnover_Player_Id` varchar(36),
  `Game_Id` varchar(36) NOT NULL,
  `IsShotClockOverride` tinyint NOT NULL,  
  `IsOpponentAttack` tinyint NOT NULL,  
  `IsFirstHalf` tinyint NOT NULL,  
  `ZoneStartFunction` varchar(1) NOT NULL,  
  PRIMARY KEY (`Id`),
  unique key (`ClusteredIndex`),
  CONSTRAINT FOREIGN KEY (`Goal_Id`) REFERENCES `Attack_Goal` (`Id`),
  CONSTRAINT FOREIGN KEY (`Game_Id`) REFERENCES `Game` (`Id`),
  CONSTRAINT FOREIGN KEY (`Turnover_Player_Id`) REFERENCES `Player` (`Id`)
);

CREATE TABLE `Attack_Rebound` (
  `Id` varchar(36) NOT NULL,
  `Attack_Id` varchar(36) NOT NULL,
  `Player_Id` varchar(36) NOT NULL,  
  `Count` int(11) NOT NULL,  
  PRIMARY KEY (`Attack_Id`, `Player_Id`),
  CONSTRAINT FOREIGN KEY (`Player_Id`) REFERENCES `Player` (`Id`),
  CONSTRAINT FOREIGN KEY (`Attack_Id`) REFERENCES `Attack` (`Id`)
);

CREATE TABLE `Attack_Shot` (
  `Id` varchar(36) NOT NULL,
  `Attack_Id` varchar(36) NOT NULL,
  `Player_Id` varchar(36) NOT NULL,  
  `Count` int(11) NOT NULL,  
  PRIMARY KEY (`Attack_Id`, `Player_Id`),
  CONSTRAINT FOREIGN KEY (`Player_Id`) REFERENCES `Player` (`Id`),
  CONSTRAINT FOREIGN KEY (`Attack_Id`) REFERENCES `Attack` (`Id`)
);

