# 数据库结构规划

| 学籍信息 | 约束                   |
| -------- | -------------------- |
| 学号     | PK, String（varchar）  |
| 姓名     | String（varchar）      |
| 性别     | Bool (String enum？)  |
| 入学年份 | String               |
| 学院号    | FK, String (varchar)     |
| 出生日期  | Date                 |

| 学院信息 | 约束                   |
| -------- | -------------------- |
| 学院号     | PK, String（varchar）  |
| 学院名     | String（varchar）      |

| 课程信息 | 约束                 |
| -------- | -------------------- |
| 课程号   | PK, String (varchar) |
| 课程名   | String               |
| 开课学院  | FK, String (varchar) |
| 课程描述 | String NULL          |
| 学分     | double               |

| 成绩信息 | 约束                 |
| -------- | -------------------- |
| 学号     | FK, String (varchar) |
| 课程号    | FK, String (varchar) |
| 成绩     | double NULL           |




```mermaid
graph BT;
classDef ER fill:#FFCC;
classDef PK fill:#CFF,stroke:#CCF,stroke-width:2px, font-weight:bold;
classDef default fill:#FDF,stroke:#FBB,stroke-width:2px;
subgraph S
	SNO["学号"];
	SNAME["姓名"];
	SSEX["性别"];
	SSEC["入学年份"]; 
	SDEPT["学院号"];
	SBIRTH["出生日期"];
	class SNO PK;
end
class S ER;
subgraph DEPT
	DDEPT["学院号"];
	DNAME["学院名"];
	class DDEPT PK;
end
subgraph C
	CNO["课程号"];
	CNAME["课程名"];
	CDEPT["开课学院"];
	CDESC["课程描述"];
	CCREDIT["学分"];
	class CNO PK;
end
subgraph SC
	subgraph SCPK[" "]
        SCSNO["学号"];
        SCCNO["课程号"];
	end
	SCV["成绩"];
	class SCPK PK;
end
SDEPT --> DDEPT;
SCSNO --> SNO;
SCCNO --> CNO;
CDEPT --> DDEPT;

```
