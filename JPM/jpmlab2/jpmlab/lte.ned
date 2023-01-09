simple UE
{
    @display("i=device/palm");
    gates:
        input in;
        output out;
}

simple ENodeB
{
    @display("i=device/antennatower");
    gates:
        input in[3];
        output out[3];
}

simple MME
{
    @display("i=device/server2");
    gates:
        input in[3];
        output out[3];
}

simple HSS
{
    @display("i=device/server2");
    gates:
        input in;
        output out;
}

simple SGW
{
    @display("i=device/server2");
    gates:
        input in[4];
        output out[4];
}

simple PGW
{
    @display("i=device/server2");
    gates:
        input in[3];
        output out[3];
}

simple PCRF
{
    @display("i=device/server2");
    gates:
        input in[2];
        output out[2];
}

simple INTERNET
{
    @display("i=misc/cloud");
    gates:
        input in;
        output out;
}

network LTE
{
    @display("bgb=700,428");
    submodules:
        ue: UE {
            @display("p=56,269");
        }
        enodeb: ENodeB {
            @display("p=212,269");
        }
        mme: MME {
            @display("p=212,144");
        }
        hss: HSS {
            @display("p=336,144");
        }
        sgw: SGW {
            @display("p=336,269");
        }
        pgw: PGW {
            @display("p=468,269");
        }
        pcrf: PCRF {
            @display("p=468,144");
        }
        internet: INTERNET {
            @display("p=612,269");
        }

    connections:
        ue.out --> {  delay = 1000ms; } --> enodeb.in[0];
        enodeb.out[0] --> {  delay = 1000ms; } --> ue.in;

        enodeb.out[1] --> {  delay = 1000ms; } --> mme.in[0];
        mme.out[0] --> {  delay = 1000ms; } --> enodeb.in[1];

        enodeb.out[2] --> {  delay = 1000ms; } --> sgw.in[0];
        sgw.out[0] --> {  delay = 1000ms; } --> enodeb.in[2];

        mme.out[1] --> {  delay = 1000ms; } --> sgw.in[1];
        sgw.out[1] --> {  delay = 1000ms; } --> mme.in[1];

        mme.out[2] --> {  delay = 1000ms; } --> hss.in;
        hss.out --> {  delay = 1000ms;@display("ls=,,s"); } --> mme.in[2];

        sgw.out[2] --> {  delay = 1000ms; } --> pcrf.in[0];
        pcrf.out[0] --> {  delay = 1000ms; } --> sgw.in[2];

        sgw.out[3] --> {  delay = 1000ms; } --> pgw.in[0];
        pgw.out[0] --> {  delay = 1000ms; } --> sgw.in[3];

        pcrf.out[1] --> {  delay = 1000ms; } --> pgw.in[1];
        pgw.out[1] --> {  delay = 1000ms; } --> pcrf.in[1];

        pgw.out[2] --> {  delay = 1000ms; } --> internet.in;
        internet.out --> {  delay = 1000ms; } --> pgw.in[2];
}
