using Microsoft.EntityFrameworkCore;
using ShopHub.Model.Context;
using ShopHub.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ShopHub.Tests
{
    public class UserTests
    {
        [Fact]
        public void User_AddUser_UserSuccessfullyAddedToTheDatabaseWithAllSpecifiedDetails()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Model.Context.ShopHubContext>()
                .UseInMemoryDatabase(databaseName: "AddUserToDatabaseTest")
                .Options;

            User user = new User("John", "Smith", "Password1", new UserType());

            User userRecordFromDatabase;


            //Act
            using (var dbContext = new ShopHubContext(options))
            {
                dbContext.Add(user);
                dbContext.SaveChanges();

                userRecordFromDatabase = dbContext.Users.First();
            }


            //Assert
            Assert.NotNull(userRecordFromDatabase);
            Assert.Equal("John", userRecordFromDatabase.FirstName);
            Assert.Equal("Smith", userRecordFromDatabase.LastName);
            Assert.Equal("Password1", userRecordFromDatabase.Password);
        }

        [Fact]
        public void User_AddTwoUsersAndRemoveOneUser_OnlyOneUserRemainsInTheDatabaseWithAllSpecifiedDetails()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ShopHubContext>()
                .UseInMemoryDatabase(databaseName: "AddTwoUsersAndRemoveOneUserTest")
                .Options;

            User user1 = new User("John", "Smith", "JohnPassword", new UserType());
            User user2 = new User("Julliet", "Smith", "JullietPassword", new UserType());

            List<User> databaseUserRecords;


            //Act
            using (var dbContext = new ShopHubContext(options))
            {
                //Add both users to the datbaase
                dbContext.Add(user1);
                dbContext.Add(user2);
                dbContext.SaveChanges();

                //Remove first user (John) from the database
                dbContext.Remove(user1);
                dbContext.SaveChanges();

                databaseUserRecords = dbContext.Users.ToList();
            }


            //Assert
            Assert.NotNull(databaseUserRecords);
            Assert.Single(databaseUserRecords);

            //Check only Julliet remains
            Assert.Equal("Julliet", databaseUserRecords.First().FirstName);
            Assert.Equal("Smith", databaseUserRecords.First().LastName);
            Assert.Equal("JullietPassword", databaseUserRecords.First().Password);
            Assert.DoesNotContain(databaseUserRecords, x => x.FirstName.Equals("John"));
        }
    }
}
