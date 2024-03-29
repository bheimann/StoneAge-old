﻿using StoneAge.Core.Models;
using System;

namespace StoneAge.Core.Exceptions
{
    public class InvalidSpaceForCardsException : Exception
    {
        public InvalidSpaceForCardsException(BoardSpace space) :
            base($"Cannot find card for a non card slot space {space}.")
        {
        }
    }
}
