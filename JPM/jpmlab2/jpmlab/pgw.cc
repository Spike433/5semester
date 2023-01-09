/*
 * pgw.cc
 *
 *  Created on: Jun 19, 2019
 *      Author: Bart
 */

#include <string.h>
#include <omnetpp.h>

using namespace omnetpp;

class PGW : public cSimpleModule {
protected:
    // The following redefined virtual function holds the algorithm.
    virtual void initialize() override;
    virtual void handleMessage(cMessage *msg) override;
};

// The module class needs to be registered with OMNeT++
Define_Module(PGW);

void PGW::initialize() {}

void PGW::handleMessage(cMessage *msg) {
    if (msg->getKind() == 3){
        EV << "P-GW prihvaca create session request te dodijeljuje IP adresu UE-u\n";
        cMessage *createSessionRes = new cMessage("CreateSessionResponse", 4);
        send(createSessionRes, "out", 0);
        EV << "P-GW kreira PDN konekciju za UE te salje create session response poruku S-GW-u otkud je dosao create session request\n";
    }
    if (msg->getKind() == 10){
        cMessage *download = new cMessage("DownloadPacket", 13);
        send(download, "out", 0);
    }
}


