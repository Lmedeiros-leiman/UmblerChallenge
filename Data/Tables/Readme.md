# Database Tables Folder

From some past personal experiences i've figured the database is often better explored if left as a separate "inner" project. The data needs to be stored in a dedicated way to avoid "garbage" data. the way to do it is by having services classes that directly contact the database instead of passing the context to other parts of the application.


these services assumes the data is already cleanned up filtered, since its the forms job to clean it up when the user information arives into the server.