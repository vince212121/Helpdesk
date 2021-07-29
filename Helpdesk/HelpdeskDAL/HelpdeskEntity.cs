/*
\file:      HelpdeskEntity
\author:    Vincent Li
\purpose:   This is the file that the things that use id or timer will inherit from
*/
using System.ComponentModel.DataAnnotations;

namespace HelpdeskDAL
{
    public class HelpdeskEntity
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] Timer { get; set; } // c# stores stuff as bytes and to put it into js with JSON, you need to use base64 encoding to convert it to ascii to a string then it can be saved without the loss of data in the huge line of text
    }
}
