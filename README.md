RocketNozzleNormalShock
-------------------------

C# cmdline utility that handles calculates the location where a normal shock will appear inside a rocket nozzle.
-------------------------
Inputs:

**Nozzle Area Ratio:** ratio of exit area of nozzle to throat area  
**Ratio of Specific Heats:** gas property of exhaust; > 1 but <= 1.6666666...

**Chamber Pressure (kPa):** internal pressure of the nozzle chamber; equivalent to stagnation pressure of the flow prior to the shock  
**Ambient Pressure (kPa):** pressure outside the nozzle; the root cause of the normal shock in the nozzle

Outputs:

**Area Ratio where normal shock occurs**