// <auto-generated />
namespace Jabbot.TwitterNotifierSprocket.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    
    public sealed partial class newfield : IMigrationMetadata
    {
        string IMigrationMetadata.Id
        {
            get { return "201112131948322_new-field"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return "H4sIAAAAAAAEAOy9B2AcSZYlJi9tynt/SvVK1+B0oQiAYBMk2JBAEOzBiM3mkuwdaUcjKasqgcplVmVdZhZAzO2dvPfee++999577733ujudTif33/8/XGZkAWz2zkrayZ4hgKrIHz9+fB8/Iv7Hv/cffPx7vFuU6WVeN0W1/Oyj3fHOR2m+nFazYnnx2Ufr9nz74KPf4+g3Th6fzhbv0p807fbQjt5cNp99NG/b1aO7d5vpPF9kzXhRTOuqqc7b8bRa3M1m1d29nZ2Du7s7d3MC8RHBStPHr9bLtljk/Af9eVItp/mqXWflF9UsLxv9nL55zVDTF9kib1bZNP/so+9kk0nVjt9cFW2b1y+qtjgv8vr1qq6mb/N2LAA+So/LIiPkXufl+XtiuvMQmH5kcSAsTgnb9vrN9SpnTD776Ksmr/0W1Ob3yq+DD+ijl3W1yuv2+lV+7r13NvsovRu+e7f7sn218x5Q+Oyjs2V7b++j9MW6LLNJSR+cZ2WTf5SuPn30uq3q/PN8mddZm89eZqARzdfZLOchKCkerT69HTUe3t3ZAzXuZstl1WYtTX4P+Q6qmJ8a+OJPg/HrtiaG+ih9VrzLZ8/z5UU7t1h/kb0zn9CvH6VfLQviP3qprde5P0r5e3PvyhY/Z/0/z5pWeHKq1BIEntJ0vCmAUG/WNgM8XaKxD7IxMJ9UVZlny/cGCRyPp21xyQzxgfgB2NmSQOUA8cHgnhYNWgvE9xzp47tOTjdKL+myL/I2m2Vt9nWE+OsI8P9HhJe157ezZv6zLDibp4rMQZsVRIZQqLu6/lW+qpqCaHbdmUWB8jpvPe1JvOS6FLswZi3epUj3ZY9ZYjB8XooO0Q7GGbW7YtWM9bs7YP4ef5GtVkR9zxzqJ+lrsYUn26/f37gtBMbdaROxcRZb2xPRN7vIO99S14Tps6JuWhL1bJKBH05mi16zrz11pn9/Brui6SbDtMbvzgzd6CaM+3acASu4ZzTyBckeE0FJQKjFzL+++HqalVk9YLxPqnK9WG5yBDZB6dhVH1jnq9vD7FlLH2rvy9vD7VtBH3D/29tDjppDH3i0we3hh7axi7X75v0g+gayC9P/7vZQu3bSh9r9rg/18d0Oh3cly1PP2rJj77pyeispDlTpNy/Mg2ad4d8k0xvfHpqGrli/n0h71tYH4n38Q566ns3qNrG9W9vVsVGP1V4EhozH343jegZEmnyUEpEuixmMx+vrps0XYzQYv/5F5UlZ0Hhdgy+yZXGeN+2b6m2OmJTs29eP+6yz1DSz8v/NwV8BEtzoKb6n2x0N2paXWT2dZ/XWInt3530DoYFA7INgDgVXJLM50gnvPewNwdWkaN8bXCyw+tq4xQOrrw0uHljdPMr/bwRVPytC4enh92be9wtw+s50iJhV2b4GGopnRI8Sq0wqQlwQ1Ajo9qHOTZFOrJNvIhTqm4/Hd/1k4eOneVNcOBBIHS7zKUTXATVtzpbnlZlTGqWPkWnSnXLF/7gmlZBNW/p6mjcNh8A/mZVrjHIxyWdnyy/X7WrdHjdNvpiUgUP4+O7m/jneC3F+/OWKlc83MQRCs4CW+HL5ZF2UM4v3s4hsD4AAO6ocEVaUAiBwF9cW0otqeUtASr6n+SpfQgrf5ItVScCaL5evs8t8GLebaRhS7PHTIruos4VPQflEMXmdUc9eF9SB/4brj/4kdqVM9dH/EwAA//9OoqbmHhcAAA=="; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return "H4sIAAAAAAAEAOy9B2AcSZYlJi9tynt/SvVK1+B0oQiAYBMk2JBAEOzBiM3mkuwdaUcjKasqgcplVmVdZhZAzO2dvPfee++999577733ujudTif33/8/XGZkAWz2zkrayZ4hgKrIHz9+fB8/Iv7Hv/cffPx7vFuU6WVeN0W1/Oyj3fHOR2m+nFazYnnx2Ufr9nz74KPf4+g3Th6fzhbv0p807fbQjt5cNp99NG/b1aO7d5vpPF9kzXhRTOuqqc7b8bRa3M1m1d29nZ2Du7s7d3MC8RHBStPHr9bLtljk/Af9eVItp/mqXWflF9UsLxv9nL55zVDTF9kib1bZNP/so+9kk0nVjt9cFW2b1y+qtjgv8vr1qq6mb/N2LAA+So/LIiPkXufl+XtiuvMQmH5kcSAsTgnb9vrN9SpnTD776Ksmr/0W1Ob3yq+DD+ijl3W1yuv2+lV+7r13NvsovRu+e7f7sn218x5Q+Oyjs2V7b++j9MW6LLNJSR+cZ2WTf5SuPn30uq3q/PN8mddZm89eZqARzdfZLOchKCkerT69HTUe3t3ZAzXuZstl1WYtTX4P+Q6qmJ8a+OJPg/HrtiaG+ih9VrzLZ8/z5UU7t1h/kb0zn9CvH6VfLQviP3qprde5P0r5e3PvyhY/Z/0/z5pWeHKq1BIEntJ0vCmAUG/WNgM8XaKxD7IxMJ9UVZlny/cGCRyPp21xyQzxgfgB2NmSQOUA8cHgXuRXz4q8tKz+Q5u4p0WDxjKU9yTx47tOQWxUG6REv8jbbJa12dfRHl9Hc/x/RGuw2v521sx/lid+81SRHWqzgsgQapOukXmVr6qmIJpdd2ZRoLzOW09tEy+5LsUgjdl8dCnSfdljlhgMn5eiQ7SDcdb0rphTY3bvDtjdx19kqxVR37PD+kn6Wozwyfbr97eqC4Fxd9pEjKvF1vZE9M0u8s631DVh+qyom5Z0TDbJwA8ns0Wv2deeOtO/P4Nd0XSTYVrjd2f/bvRPxn0HggEruGc08gXJHhNBSUCoxfwOffH1NCuzesBrOKnK9WK5yQPZBKVj0H1gna9uD7Nnpn2ovS9vD7dvfn3A/W9vDzlqh33g0Qa3hx8a5S7W7pv3g+hb5i5M/7vbQ3UG2ofnPr09pK7F9eF1v+tDfXy3IytdGfUUvbbsWM6uxN9KHwRK+ZtXC4MOAsO/STtsfHtoGroK4v2Ug2e3fSDexz/kqetZv24T27u1gh1r91gtT2ASefzdULRniqTJRykR6bKYwQy9vm7afDFGg/HrX1SelAWN1zX4IlsW53nTvqne5giryVJ+/dDVul1NMyv/3xy/FiDBjT7ne0YO0bhzeZnV03lWby2yd3feNyQYiCU/COZQfEgymyMj8t7D3hAfTor2vcHFYsOvjVs8Nvza4Lqx4QdNRDzeu5lk/9+I9X5WJMxT6u89Ae8Xd/V9/BAxq/99dTYUZolSJr6bVIS4IKiB2e0jsJsCsFgn30SE1rdFj+/6ydPHT/OmuHAgkEpd5lPoAQfUtDlbnldmTmmUPkamSXfKFf/jmvRLNm3p62neNByZ/2RWrjHKxSSfnS2/XLerdXvcNPliUgZ+6uO7m/vnMDTE+fGXK9Zk38QQCM0CKufL5ZN1AeWheD+LyPYACLCjyhFhRZkJAndxbSG9qJa3BKTke5qv8iWk8E2+WJUErPly+Tq7zIdxu5mGIcUePy2yizpb+BSUTxST19liVXpdUAf+G64/+pPYlTL3R/9PAAAA//9l9m0bLhgAAA=="; }
        }
    }
}
