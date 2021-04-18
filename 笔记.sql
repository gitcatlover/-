PRINT DATALENGTH('22');
PRINT ABS(-5);
PRINT ROUND(2.34, 1);

SELECT * FROM dbo.Course
ORDER BY CId DESC,TId ASC

--至少比子查询里面集合的某一个大
--some=in ，not some!=not in
SELECT CId FROM course
WHERE cid >SOME(SELECT CId FROM sc WHERE cid>1)
--all比所有的都大
SELECT CId FROM course
WHERE cid >ALL(SELECT CId FROM sc WHERE cid>1)
--平均分最高的SID
SELECT SId,AVG(score) avg_cid FROM dbo.SC
GROUP BY SId
HAVING AVG(score)>= ALL(SELECT AVG(score) FROM dbo.SC GROUP BY SId)
--a exists(b)查询,每次查出结果a里的每一条，都要再去查结果b，效率低 

--数据：用来表示事务的符号
--数据库：长期保存在计算机内有组织的可共享的数据集合
--指定主键后，系统会自动建一个主索引
--视图只是看到数据结果集，数据存在于原来的数据表里，只能看不能存。就像通过窗户看教室里的学生


--用户WANG定义一个学生-课程模式 S-T
create schema 'S-T' AUTHORIZATION WANG;

--ALTER ANY CONNECTION  关闭当前链接  KILL 5 关闭id为5的会话
--ALTER LOGIN sqltest WITH PASSWORD='123456'
--ALTER LOGIN sqltest DISABLE  禁用登录，ENABLE开启

--查询可帮助确定想要终止的 session_id：
SELECT conn.session_id, host_name, program_name,
    nt_domain, login_name, connect_time, last_request_end_time 
FROM sys.dm_exec_sessions AS sess
JOIN sys.dm_exec_connections AS conn
   ON sess.session_id = conn.session_id;

CREATE TABLE Student
(
    Sno CHAR(9) PRIMARY KEY,
    Sname CHAR(20)
        UNIQUE,
    Sage SMALLINT,
    Sdept CHAR(9)
);
CREATE TABLE Course
  (
     Cno     CHAR(4) PRIMARY KEY,
     Cname   CHAR(40),
     Cpno    CHAR(4),--选修课
     Ccredit SMALLINT,
     FOREIGN KEY(Cpno) REFERENCES Course(Cno)
  ) 

  CREATE TABLE SC
  (
     Sno   CHAR(9),
     Cno   CHAR(4),
     Grade SMALLINT,
     PRIMARY KEY(Sno, Cno),
     FOREIGN KEY(Sno) REFERENCES Student(Sno),
     FOREIGN KEY(Cno) REFERENCES Course(Cno)
  ) 


--索引是依赖于表的，可以看作是访问数据的一种路径。primary key，unique都会建索引
--索引一般实现：B+树 动态平衡，HASH索引 查找速度快

CREATE CLUSTER INDEX Stusage ON Student(Sage)--聚集索引，把同龄学生的记录都存放到一起，查找迅速

select 100- sage,*from student --查询经过计算的值
select 'year of birth:' birth,2021-sage birthday,LOWER(sdept) Sdept from student 
select*from student where Sname like '刘__' --查询出刘开头，至少有两个汉字的名字，_占一个字符(数据库编码不同，占用字符不同)

--查询每个学生成绩超过他选修课平均分的学生
SELECT *FROM SC x
WHERE grade>=(SELECT AVG(Grade) FROM sc y WHERE x.Sno=y.Sno)

--查询其他系的学生，他比计算机系任意一个学生的年龄小
SELECT *FROM dbo.Student 
WHERE  sage<ANY(SELECT sage FROM student WHERE sdept='CS')
AND sdept <>'CS'


GRANT SELECT,INSERT ON student TO sqltest; --student查询权写入限开放给sqltest用户
GRANT SELECT ON student TO PUBLIC; --student查询权限开放给所有账号
GRANT ALL PRIVILEGES ON sc TO sqltest;
GRANT UPDATE(Sname),SELECT ON student TO sqltest;--给予sqltest查询student和更新sname字段的权限
REVOKE UPDATE(Sname) ON student FROM sqltest; --取消sqltest更新sname字段的权限，也可用TO
REVOKE DELETE,SELECT ON student FROM PUBLIC --取消所有用户对student的删除权限

--数据库审计，记录数据库操作
-- 1、创建 Server Audit
USE [master]
CREATE SERVER AUDIT[Audit-AuditTest]
TO FILE 
(
FILEPATH =N'D:\SqlAudit',
MAXSIZE=1GB,
MAX_ROLLOVER_FILES=10,--系统中最多保留10个文件
RESERVE_DISK_SPACE=OFF --预先分配审核文件到MaxSize
)
WITH(
QUEUE_DELAY=1000,--被强制审核的毫秒间隔
ON_FAILURE=CONTINUE --审核写入数据失败时
)
alter server audit [Audit-AuditTest] with (state=on)
--2，数据库审计
USE [MyTest]
CREATE DATABASE AUDIT SPECIFICATION[DB_AuditTest]
FOR SERVER AUDIT [Audit-AuditTest]
ADD (INSERT,UPDATE,DELETE ON OBJECT::[dbo].[student] BY [PUBLIC])
WITH (STATE=ON)
--AUDIT ALTER,UPDATE ON student
--NOAUDIT ALTER,UPDATE ON student
SELECT* FROM dbo.Student
UPDATE dbo.Student SET Sname='刘M' WHERE Sno=3

SELECT event_time, action_id,succeeded, session_id,server_instance_name, 
session_server_principal_name,object_name, statement, file_name, audit_file_offset
FROM sys.fn_get_audit_file 
('D:\SqlAudit\Audit-AuditTest_69EFF93F-B7CB-4454-AB9D-0B2383E70923_0_132631929123830000.sqlaudit',default,default)

--表的主键不能重复不能为空就是数据的完整性
--一般索引B+树根节点是存放在内存里，三层的B+树索引插入数据需要两次IO操作

--参照完整性检查违约处理
CREATE TABLE SC
        (Sno   CHAR(9)  NOT NULL,
         Cno   CHAR(4)  NOT NULL,
         Grade  SMALLINT,
         PRIMARY KEY(Sno,Cno), 				
         FOREIGN KEY (Sno) REFERENCES Student(Sno) 
		ON DELETE CASCADE     /*级联删除SC表中相应的元组*/
                ON UPDATE CASCADE, /*级联更新SC表中相应的元组*/
         FOREIGN KEY (Cno) REFERENCES Course(Cno) 	                    
               ON DELETE NO ACTION 	
               /*当删除course 表中的元组造成了与SC表不一致时拒绝删除*/
               ON UPDATE CASCADE   
      	/*当更新course表中的cno时，级联更新SC表中相应的元组*/
        );
