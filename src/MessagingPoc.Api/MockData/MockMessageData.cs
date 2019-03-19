using System.Collections.Generic;
using MessagingPocApi.Models;

public class MockMessages
{
    public static MockMessages Current { get; } = new MockMessages();
    public List<FolderDto> Folders { get; set; }

    public MockMessages()
    {
        Folders = new List<FolderDto>
        {
            new FolderDto
            {
                id = 1,
                name = "Inbox",
                messages = new List<MessageDto>
                {
                    new MessageDto
                    {
                        id = 1,
                        from = "abc@gmail.com",
                        to = "zyx@yahoo.com",
                        subject = "mock data message1",
                        body = "this is the body of my first message"
                    },
                    new MessageDto
                    {
                        id = 2,
                        from = "def@gmail.com",
                        to = "wvu@yahoo.com",
                        subject = "mock data message2",
                        body = "this is the body of my second message"
                    },
                    new MessageDto
                    {
                        id = 3,
                        from = "ghi@gmail.com",
                        to = "tsr@yahoo.com",
                        subject = "mock data message2",
                        body = "this is the body of my third message"
                    }
                }
            },
            new FolderDto
            {
                id = 2,
                name = "Sent",
                messages = new List<MessageDto>
                {
                    new MessageDto
                    {
                        id = 1,
                        from = "sam@gmail.com",
                        to = "zyx@yahoo.com",
                        subject = "sent mock data message1",
                        body = "this is the body of my first sent message"
                    },
                }
            }
        };
    }
}
