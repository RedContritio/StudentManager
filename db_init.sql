use sm;

SELECT * FROM sm.s where SUSER is null;

drop view view_s_s;
create view view_s_s as (select SNO, SNAME, SSEX, SSEC, DNAME, SBIRTH, SLOC, STEL from s, d where
	(
		(
			(SUSER is null and user() REGEXP (concat("^",SNAME,'@[^@]+$'))) or
			user() REGEXP (concat("^",SUSER,'@[^@]+$'))) and
	s.SDEPT = d.DNO));

drop view view_sc_s;
create view view_sc_s as
	(select SNAME, CNAME, CCREDIT, CSCORE SCORE from sc, s, c where
		(
			(SUSER is null and user() REGEXP (concat("^",SNAME,'@[^@]+$'))) or
			user() REGEXP (concat("^",SUSER,'@[^@]+$'))) and
        s.SNO = sc.SNO and c.CNO = SC.CNO);

create view view_c_s as
	(select CNO, CNAME, DNAME, CDESC, CCREDIT from c, d where
		c.CDEPT = d.DNO);


drop procedure if exists grant_init;
delimiter $
create procedure grant_init (`username` varchar(16))
BEGIN
	SET @query = concat("GRANT select on view_c_s to ", `username`, "@'%'");
    PREPARE stmt FROM @query;
	EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
	SET @query = concat("GRANT select on d to ", `username`, "@'%'");
    PREPARE stmt FROM @query;
	EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
	SET @query = concat("GRANT select(SNO) on s to ", `username`, "@'%'");
    PREPARE stmt FROM @query;
	EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
	SET @query = concat("GRANT update(SNAME, SBIRTH, SSEX, SUSER) on s to ", `username`, "@'%'");
    PREPARE stmt FROM @query;
	EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
	SET @query = concat("GRANT select on view_s_s to ", `username`, "@'%'");
    PREPARE stmt FROM @query;
	EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
	SET @query = concat("GRANT select on view_sc_s to ", `username`, "@'%'");
    PREPARE stmt FROM @query;
	EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
END $
delimiter ;

call grant_init('张三');
call grant_init('李四');

alter table c drop constraint c_fk_dept;
alter table c add constraint c_fk_dept foreign key (CDEPT) references d(DNO) on update cascade on delete restrict;

select concat(SNAME, "@") from s;

select Select_priv, Insert_priv, Update_priv, Delete_priv from mysql.user where
	user() REGEXP (concat('^',User,'@[^@]+$'));
    
select * from s;

update s set SNAME='李六', SSEX='女' where SNO='10002';

select COUNT(*) from sc where SNO='10001';

select * from sc;

select s.SNO `学号`, SNAME `姓名`, COUNT(*) `考试课程数量`, avg(CSCORE) `平均成绩`, min(CSCORE) `最低分`, max(CSCORE) `最高分`  from s, sc where s.SNO = sc.SNO and CSCORE is not null group by s.SNO;
select CNAME `课程`, COUNT(*) `考试人次`, avg(CSCORE) `平均成绩`, min(CSCORE) `最低分`, max(CSCORE) `最高分`  from c, sc where c.CNO = sc.CNO and CSCORE is not null group by c.CNO;
select DNAME `专业`, COUNT(*) `考试人次`, avg(CSCORE) `平均成绩`, min(CSCORE) `最低分`, max(CSCORE) `最高分` from s, sc, d where s.SNO = sc.SNO and S.SDEPT = d.DNO and CSCORE is not null group by SDEPT;
select CNAME `课程`, DNAME `专业`, COUNT(*) `考试人次`, avg(CSCORE) `平均成绩`, min(CSCORE) `最低分`, max(CSCORE) `最高分` from s, c, sc, d where s.SNO = sc.SNO and c.CNO = sc.CNO and S.SDEPT = d.DNO and CSCORE is not null group by SDEPT, c.CNO;