
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SFLCC_Demo_Website.Models
{
    public class Speaker
    {
        [Key]
        public int SpeakerId { get; set; }

        public string SpeakerName { get; set; }

        public string SpeakerImage { get; set; }
        public string Company { get; set; }
        public string CompanyImage { get; set; }
        public string SpeakerBio { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }
    }

    public class Event
    {
        [Key]
        public int EventId { get; set; }

        public string EventName { get; set; }
        [DisplayFormat(DataFormatString = "{0: MM/dd/YYYY hh:mm tt}")]
        public DateTime EventStartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0: MM/dd/YYYY hh:mm tt}")]
        public DateTime EventEndDate { get; set; }
        public virtual Location Location { get; set; }

    }

    public class Location
    {

        [Key]
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
    }

    public class Building
    {

        [Key]
        public int BuildingId { get; set; }
        public virtual Location Location { get; set; }
        public string BuildingName { get; set; }
        public string BuildingSortName { get; set; }
        public string BuildingIdentifier { get; set; }
        public int NumberOfFloors { get; set; }
        public int HasElevator { get; set; }
        public int HasChairLift { get; set; }
        public bool HandicapAccesible { get; set; }

    }
    public class Room
    {

        [Key]
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int Floor { get; set; }
        public bool HandicapAccesible { get; set; }
        public virtual Building Bulding { get; set; }

    }

    public class Attendee
    {

        [Key]
        public int AttendeeId { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }

    }
    public class Session
    {
        [Key]
        public int SessionId { get; set; }
        public virtual Speaker Speaker { get; set; }
        public virtual TrackSlot Slot { get; set; }
        public string SessionName { get; set; }
        public string SessionDescription { get; set; }

    }

    public class SessionSlot
    {
        [Key]
        public int SessionSlotId { get; set; }
         [DisplayFormat(DataFormatString = "{0:hh:mm tt}")]
        public DateTime SessionStart { get; set; }

         [DisplayFormat(DataFormatString = "{0:hh:mm tt}")]
        public DateTime SessionEnd { get; set; }
    }

    public class Track
    {
        [Key]
        public int TrackId { get; set; }
        public virtual Event Event { get; set; }
        public virtual Room Room { get; set; }
        public string TrackName { get; set; }
    }

    public class TrackSlot
    {
        [Key]
        public int TrackSlotId { get; set; }
        public virtual Track Track { get; set; }
        public virtual SessionSlot Slot { get; set; }
    }

    public class AgendaViewModel
    {
        public ICollection<Track> Tracks { get; set; }
        public ICollection<Session> Sessions { get; set; }
    }

}
