import json


def delete_file(filename):
    open(filename, "w").close()


def initialize(filename):
    with open(filename, encoding="utf-8", mode="w") as file:
        file.write("USE[Couponel]\nGO\n\n")


def finalize(filename):
    with open(filename, encoding="utf-8", mode="a") as file:
        file.write("GO\n")


def init():
    initialize("output/Address.sql")
    initialize("output/Faculty.sql")
    initialize("output/University.sql")

    initialize("output/Admin.sql")
    initialize("output/Student.sql")
    initialize("output/Offerer.sql")
    initialize("output/User.sql")


def finish():
    finalize("output/Address.sql")
    finalize("output/Faculty.sql")
    finalize("output/University.sql")

    finalize("output/Admin.sql")
    finalize("output/Student.sql")
    finalize("output/Offerer.sql")
    finalize("output/User.sql")


def generate_institutions():
    address_list = []
    university_list = []
    faculty_list = []
    with open("data/university.json", encoding='utf-8') as data_file:
        universities = json.load(data_file)
        for university in universities:
            address_list.append(university["UniversityAddress"])

            university_list.append({
                "Id": university["Id"],
                "Name": university["Name"],
                "Email": university["Email"],
                "PhoneNumber": university["PhoneNumber"],
                "AddressId": university["UniversityAddress"]["Id"]
            })

            for faculty in university["Faculties"]:
                address_list.append(faculty["FacultyAddress"])

                faculty_list.append({
                    "Id": faculty["FacultyId"],
                    "Name": faculty["Name"],
                    "Email": faculty["Email"],
                    "PhoneNumber": faculty["PhoneNumber"],
                    "UniversityId": university["Id"],
                    "AddressId": faculty["FacultyAddress"]["Id"]
                })

    # filter duplicated id in addresses, faculties and unversities
    address_list = [dict_item for i, dict_item in enumerate(address_list)
                    if dict_item["Id"] not in [address["Id"] for address in address_list[i + 1:]]]
    faculty_list = [dict_item for i, dict_item in enumerate(faculty_list)
                    if dict_item["Id"] not in [faculty["Id"] for faculty in faculty_list[i + 1:]]]
    university_list = [dict_item for i, dict_item in enumerate(university_list)
                       if dict_item["Id"] not in [university["Id"] for university in university_list[i + 1:]]]

    print(address_list)
    print(faculty_list)
    print(university_list)
    address_string_builder_sql = ""
    for address in address_list:
        address_string_builder_sql += """INSERT INTO [dbo].[Addresses]
           ([Id]
           ,[Country]
           ,[City]
           ,[Street]
           ,[Number])
        VALUES
           (CONVERT(uniqueidentifier,'{}')
           ,'{}'
           ,'{}'
           ,'{}'
           ,'{}')\n\n""".format(address["Id"],
                                address["Country"],
                                address["City"],
                                address["Street"],
                                address["Number"])

    # print(address_string_builder_sql)

    faculty_string_builder_sql = ""
    for faculty in faculty_list:
        faculty_string_builder_sql += """INSERT INTO [dbo].[Faculties]
           ([Id]
           ,[Name]
           ,[Email]
           ,[PhoneNumber]
           ,[UniversityId]
           ,[AddressId])
     VALUES
           (CONVERT(uniqueidentifier,'{}')
           ,'{}'
           ,'{}'
           ,'{}'
           ,CONVERT(uniqueidentifier,'{}')
           ,CONVERT(uniqueidentifier,'{}'))\n\n""".format(faculty["Id"],
                                faculty["Name"],
                                faculty["Email"],
                                faculty["PhoneNumber"],
                                faculty["UniversityId"],
                                faculty["AddressId"])

    # print(faculty_string_builder_sql)

    university_string_builder_sql = ""
    for university in university_list:
        university_string_builder_sql += """INSERT INTO [dbo].[Universities]
           ([Id]
           ,[Name]
           ,[Email]
           ,[PhoneNumber]
           ,[AddressId])
     VALUES
           (CONVERT(uniqueidentifier,'{}')
           ,'{}'
           ,'{}' 
           ,'{}'
           ,CONVERT(uniqueidentifier,'{}'))\n\n""".format(university["Id"],
                                university["Name"],
                                university["Email"],
                                university["PhoneNumber"],
                                university["AddressId"])
    # print(university_string_builder_sql)

    with open("output/Address.sql", mode="a", encoding="utf-8") as address_file:
        address_file.write(address_string_builder_sql)
    with open("output/Faculty.sql", mode="a", encoding="utf-8") as faculty_file:
        faculty_file.write(faculty_string_builder_sql)
    with open("output/University.sql", mode="a", encoding="utf-8") as university_file:
        university_file.write(university_string_builder_sql)


def generate_admins():
    admin_list = []
    address_list = []
    user_list = []
    with open("data/admin.json", encoding='utf-8') as data_file:
        admins = json.load(data_file)
        for admin in admins:
            address_list.append(admin["Address"])
            user_list.append(admin["User"])
            admin_list.append({
                "Id": admin["Id"],
                "FirstName": admin["FirstName"],
                "LastName": admin["LastName"],
                "PhoneNumber": admin["PhoneNumber"],
                "UserId": admin["User"]["Id"],
                "AddressId": admin["Address"]["Id"]
            })

    # filter duplicated id in addresses, users and admins
    address_list = [dict_item for i, dict_item in enumerate(address_list)
                    if dict_item["Id"] not in [address["Id"] for address in address_list[i + 1:]]]
    user_list = [dict_item for i, dict_item in enumerate(user_list)
                 if dict_item["Id"] not in [user["Id"] for user in user_list[i + 1:]]]
    admin_list = [dict_item for i, dict_item in enumerate(admin_list)
                  if dict_item["Id"] not in [admin["Id"] for admin in admin_list[i + 1:]]]

    print(address_list)
    print(user_list)
    print(admin_list)
    address_string_builder_sql = ""
    for address in address_list:
        address_string_builder_sql += """INSERT INTO [dbo].[Addresses]
           ([Id]
           ,[Country]
           ,[City]
           ,[Street]
           ,[Number])
        VALUES
           (CONVERT(uniqueidentifier,'{}')
           ,'{}'
           ,'{}'
           ,'{}'
           ,'{}')\n\n""".format(address["Id"],
                                address["Country"],
                                address["City"],
                                address["Street"],
                                address["Number"])

    user_string_builder_sql = ""
    for user in user_list:
        user_string_builder_sql += """INSERT INTO [dbo].[Users]
           ([Id]
           ,[UserName]
           ,[Email]
           ,[PasswordHash]
           ,[Role])
     VALUES
           (CONVERT(uniqueidentifier,'{}')
           ,'{}'
           ,'{}'
           ,'{}'
           ,'{}')\n\n""".format(user["Id"],
                                user["UserName"],
                                user["Email"],
                                user["PasswordHash"],
                                user["Role"])

    admin_string_builder_sql = ""
    for admin in admin_list:
        admin_string_builder_sql += """INSERT INTO [dbo].[Admins]
           ([Id]
           ,[FirstName]
           ,[LastName]
           ,[PhoneNumber]
           ,[AddressId]
           ,[UserId])
     VALUES
           (CONVERT(uniqueidentifier,'{}')
           ,'{}'
           ,'{}' 
           ,'{}'
           ,CONVERT(uniqueidentifier,'{}')
           ,CONVERT(uniqueidentifier,'{}'))\n\n""".format(admin["Id"],
                                admin["FirstName"],
                                admin["LastName"],
                                admin["PhoneNumber"],
                                admin["AddressId"],
                                admin["UserId"])

    with open("output/Address.sql", mode="a", encoding="utf-8") as address_file:
        address_file.write(address_string_builder_sql)
    with open("output/User.sql", mode="a", encoding="utf-8") as user_file:
        user_file.write(user_string_builder_sql)
    with open("output/Admin.sql", mode="a", encoding="utf-8") as admin_file:
        admin_file.write(admin_string_builder_sql)


def generate_offerers():
    offerer_list = []
    address_list = []
    user_list = []
    with open("data/offerer.json", encoding='utf-8') as data_file:
        offerers = json.load(data_file)
        for offerer in offerers:
            address_list.append(offerer["Address"])
            user_list.append(offerer["User"])
            offerer_list.append({
                "Id": offerer["Id"],
                "FirstName": offerer["FirstName"],
                "LastName": offerer["LastName"],
                "PhoneNumber": offerer["PhoneNumber"],
                "UserId": offerer["User"]["Id"],
                "AddressId": offerer["Address"]["Id"]
            })

    # filter duplicated id in addresses, users and offerers
    address_list = [dict_item for i, dict_item in enumerate(address_list)
                    if dict_item["Id"] not in [address["Id"] for address in address_list[i + 1:]]]
    user_list = [dict_item for i, dict_item in enumerate(user_list)
                 if dict_item["Id"] not in [user["Id"] for user in user_list[i + 1:]]]
    offerer_list = [dict_item for i, dict_item in enumerate(offerer_list)
                  if dict_item["Id"] not in [offerer["Id"] for offerer in offerer_list[i + 1:]]]

    print(address_list)
    print(user_list)
    print(offerer_list)
    address_string_builder_sql = ""
    for address in address_list:
        address_string_builder_sql += """INSERT INTO [dbo].[Addresses]
           ([Id]
           ,[Country]
           ,[City]
           ,[Street]
           ,[Number])
        VALUES
           (CONVERT(uniqueidentifier,'{}')
           ,'{}'
           ,'{}'
           ,'{}'
           ,'{}')\n\n""".format(address["Id"],
                                address["Country"],
                                address["City"],
                                address["Street"],
                                address["Number"])

    user_string_builder_sql = ""
    for user in user_list:
        user_string_builder_sql += """INSERT INTO [dbo].[Users]
           ([Id]
           ,[UserName]
           ,[Email]
           ,[PasswordHash]
           ,[Role])
     VALUES
           (CONVERT(uniqueidentifier,'{}')
           ,'{}'
           ,'{}'
           ,'{}'
           ,'{}')\n\n""".format(user["Id"],
                                user["UserName"],
                                user["Email"],
                                user["PasswordHash"],
                                user["Role"])

    offerer_string_builder_sql = ""
    for offerer in offerer_list:
        offerer_string_builder_sql += """INSERT INTO [dbo].[Offerers]
           ([Id]
           ,[FirstName]
           ,[LastName]
           ,[PhoneNumber]
           ,[AddressId]
           ,[UserId])
     VALUES
           (CONVERT(uniqueidentifier,'{}')
           ,'{}'
           ,'{}' 
           ,'{}'
           ,CONVERT(uniqueidentifier,'{}')
           ,CONVERT(uniqueidentifier,'{}'))\n\n""".format(offerer["Id"],
                                offerer["FirstName"],
                                offerer["LastName"],
                                offerer["PhoneNumber"],
                                offerer["AddressId"],
                                offerer["UserId"])

    with open("output/Address.sql", mode="a", encoding="utf-8") as address_file:
        address_file.write(address_string_builder_sql)
    with open("output/User.sql", mode="a", encoding="utf-8") as user_file:
        user_file.write(user_string_builder_sql)
    with open("output/Offerer.sql", mode="a", encoding="utf-8") as offerer_file:
        offerer_file.write(offerer_string_builder_sql)


def generate_students():
    student_list = []
    address_list = []
    user_list = []
    with open("data/student.json", encoding='utf-8') as data_file:
        students = json.load(data_file)
        for student in students:
            address_list.append(student["Address"])
            user_list.append(student["User"])
            student_list.append({
                "Id": student["Id"],
                "FirstName": student["FirstName"],
                "LastName": student["LastName"],
                "PhoneNumber": student["PhoneNumber"],
                "UserId": student["User"]["Id"],
                "FacultyId": student["FacultyId"],
                "AddressId": student["Address"]["Id"]
            })

    # filter duplicated id in addresses, users and students
    address_list = [dict_item for i, dict_item in enumerate(address_list)
                    if dict_item["Id"] not in [address["Id"] for address in address_list[i + 1:]]]
    user_list = [dict_item for i, dict_item in enumerate(user_list)
                 if dict_item["Id"] not in [user["Id"] for user in user_list[i + 1:]]]
    student_list = [dict_item for i, dict_item in enumerate(student_list)
                    if dict_item["Id"] not in [student["Id"] for student in student_list[i + 1:]]]

    print(address_list)
    print(user_list)
    print(student_list)
    address_string_builder_sql = ""
    for address in address_list:
        address_string_builder_sql += """INSERT INTO [dbo].[Addresses]
               ([Id]
               ,[Country]
               ,[City]
               ,[Street]
               ,[Number])
            VALUES
               (CONVERT(uniqueidentifier,'{}')
               ,'{}'
               ,'{}'
               ,'{}'
               ,'{}')\n\n""".format(address["Id"],
                                    address["Country"],
                                    address["City"],
                                    address["Street"],
                                    address["Number"])

    user_string_builder_sql = ""
    for user in user_list:
        user_string_builder_sql += """INSERT INTO [dbo].[Users]
               ([Id]
               ,[UserName]
               ,[Email]
               ,[PasswordHash]
               ,[Role])
         VALUES
               (CONVERT(uniqueidentifier,'{}')
               ,'{}'
               ,'{}'
               ,'{}'
               ,'{}')\n\n""".format(user["Id"],
                                    user["UserName"],
                                    user["Email"],
                                    user["PasswordHash"],
                                    user["Role"])

    student_string_builder_sql = ""
    for student in student_list:
        student_string_builder_sql += """INSERT INTO [dbo].[Students]
               ([Id]
               ,[FirstName]
               ,[LastName]
               ,[PhoneNumber]
               ,[AddressId]
               ,[FacultyId]
               ,[UserId])
         VALUES
               (CONVERT(uniqueidentifier,'{}')
               ,'{}'
               ,'{}' 
               ,'{}'
               ,CONVERT(uniqueidentifier,'{}')
               ,CONVERT(uniqueidentifier,'{}')
               ,CONVERT(uniqueidentifier,'{}'))\n\n""".format(student["Id"],
                                                              student["FirstName"],
                                                              student["LastName"],
                                                              student["PhoneNumber"],
                                                              student["AddressId"],
                                                              student["FacultyId"],
                                                              student["UserId"])

    with open("output/Address.sql", mode="a", encoding="utf-8") as address_file:
        address_file.write(address_string_builder_sql)
    with open("output/User.sql", mode="a", encoding="utf-8") as user_file:
        user_file.write(user_string_builder_sql)
    with open("output/Student.sql", mode="a", encoding="utf-8") as student_file:
        student_file.write(student_string_builder_sql)


if __name__ == '__main__':
    init()
    generate_institutions()
    generate_admins()
    generate_offerers()
    generate_students()
    finish()
