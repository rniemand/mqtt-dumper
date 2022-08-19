CREATE TABLE `RawMessages` (
	`MessageId` BIGINT(20) NOT NULL AUTO_INCREMENT,
	`Retain` BIT(1) NOT NULL DEFAULT b'0',
	`Dup` BIT(1) NOT NULL DEFAULT b'0',
	`MessageExpiryInterval` INT(11) NOT NULL DEFAULT '0',
	`TopicAlias` INT(11) NULL DEFAULT NULL,
	`MessageTimeUtc` DATETIME NOT NULL DEFAULT utc_timestamp(6),
	`ParsedPayload` TEXT NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`PayloadFormatIndicator` VARCHAR(32) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`QualityOfServiceLevel` VARCHAR(32) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ResponseTopic` VARCHAR(256) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`Topic` VARCHAR(256) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ContentType` VARCHAR(64) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ClientId` VARCHAR(64) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	PRIMARY KEY (`MessageId`) USING BTREE,
	INDEX `Retain` (`Retain`) USING BTREE,
	INDEX `Dup` (`Dup`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
;
