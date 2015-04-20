# XPlaneGen

## Purpose
The main purpose of this project is to convert flight data files (currently CSV files) into a Flight Data Replay (FDR) file for XPlane to utilize.

## Process
Compute-bound processes are speed-up by converting textual data from source files into an intermediate binary form. Additional, queries against data (see XPlaneQuery) are due to this action.

## XPlaneQuery
__Status__: Experimental

This module allows for analysis of data

- Selection (e.g. "where Speed > 0")

## Limitations
Currently, only one format is supported.
