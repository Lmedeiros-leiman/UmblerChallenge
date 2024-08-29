# Database Tables Folder

From some past personal experiences i've figured the database is often better explored if left as a separate "inner" project. The data needs to be stored in a dedicated way to avoid "garbage" data. the way to do it is by having services classes that directly contact the database instead of passing the context to other parts of the application.


What bellongs to the database STAYS with the database.