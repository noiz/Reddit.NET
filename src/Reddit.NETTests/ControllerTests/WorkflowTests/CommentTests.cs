﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reddit.Controllers;
using Reddit.Controllers.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditTests.ControllerTests.WorkflowTests
{
    [TestClass]
    public class CommentTests : BaseTests
    {
        private SelfPost Post
        {
            get
            {
                return post ?? TestSelfPost();
            }
            set
            {
                post = value;
            }
        }
        private SelfPost post;

        private Comment Comment
        {
            get
            {
                return comment ?? TestComment();
            }
            set
            {
                comment = value;
            }
        }
        private Comment comment;

        private Dictionary<string, Comment> NewComments;

        public CommentTests() : base()
        {
            NewComments = new Dictionary<string, Comment>();
        }

        private SelfPost TestSelfPost()
        {
            Post = reddit.Subreddit(testData["Subreddit"]).SelfPost("Test Self Post (Now With Comments!)", "It is now: " + DateTime.Now.ToString("F")).Submit();
            return Post;
        }

        private Comment TestComment()
        {
            Comment = Post.Comment("This is a test comment.").Submit();
            return Comment;
        }

        [TestMethod]
        public void Submit()
        {
            Validate(Comment);
        }

        [TestMethod]
        public void Distinguish()
        {
            Comment.Distinguish("yes");
        }

        [TestMethod]
        public void Delete()
        {
            Comment.Delete();
        }

        [TestMethod]
        public async Task DeleteAsync()
        {
            await Comment.DeleteAsync();
        }

        [TestMethod]
        public void Report()
        {
            Comment.Report("This is a test (additional info).", "", "This is a test (custom).", false, "This is a test (other).", "This is a test (reason).",
                "This is a test (rule reason).", "This is a test (site reason).", "");
        }

        [TestMethod]
        public async Task ReportAsync()
        {
            await Comment.ReportAsync("This is a test (additional info).", "", "This is a test (custom).", false, "This is a test (other).", "This is a test (reason).",
                "This is a test (rule reason).", "This is a test (site reason).", "");
        }

        [TestMethod]
        public void Edit()
        {
            Validate(Comment.Edit("This comment has been edited."));
        }

        [TestMethod]
        public async Task EditAsync()
        {
            await Comment.EditAsync("This comment has been edited asynchronously.");
        }

        [TestMethod]
        public void Reply()
        {
            Validate(Comment.Reply("This is a comment reply.").Reply("This is another reply.").Reply("This is yet another reply."));
        }

        [TestMethod]
        public async Task ModifyAsync()
        {
            await Comment.SubmitAsync();
            await Comment.ReportAsync("This is a test (additional info).", "", "This is a test (custom).", false, "This is a test (other).", "This is a test (reason).",
                "This is a test (rule reason).", "This is a test (site reason).", "");
            await Comment.SaveAsync("RDNTestCat");
            await Comment.UnsaveAsync();
            await Comment.DisableSendRepliesAsync();
            await Comment.EnableSendRepliesAsync();
        }

        [TestMethod]
        public void Remove()
        {
            Comment.Remove();
        }

        [TestMethod]
        public void MonitorNewComments()
        {
            Comment.Comments.GetNew();  // This call prevents any existing "new"-sorted comments from triggering the update event.  --Kris
            Comment.Comments.MonitorNew();
            Comment.Comments.NewUpdated += C_NewCommentsUpdated;

            for (int i = 1; i <= 10; i++)
            {
                // Despite what VS says, we don't want to use await here.  --Kris
                Comment.ReplyAsync("Comment #" + i.ToString());
            }

            DateTime start = DateTime.Now;
            while (NewComments.Count < 10
                && start.AddMinutes(1) > DateTime.Now) { }

            Comment.Comments.NewUpdated -= C_NewCommentsUpdated;
            Comment.Comments.MonitorNew();

            Assert.IsTrue(NewComments.Count >= 7);
        }

        // When a new comment is detected in MonitorNewComments, this method will add it/them to the list.  --Kris
        private void C_NewCommentsUpdated(object sender, CommentsUpdateEventArgs e)
        {
            foreach (Comment comment in e.Added)
            {
                if (!NewComments.ContainsKey(comment.Fullname))
                {
                    NewComments.Add(comment.Fullname, comment);
                }
            }
        }
    }
}
