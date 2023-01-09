/*
 * mme.cc
 *
 *  Created on: Jun 19, 2019
 *      Author: Bart
 */

#include <string.h>
#include <omnetpp.h>

using namespace omnetpp;

class MME : public cSimpleModule {
    protected:
        // The following redefined virtual function holds the algorithm.
        virtual void initialize() override;
        virtual void handleMessage(cMessage *msg) override;
};

// The module class needs to be registered with OMNeT++
Define_Module(MME);

void MME::initialize() {}

void MME::handleMessage(cMessage *msg) {
    if (msg->getKind() == 1){
        cMessage *authDataReq = new cMessage("AuthenticationDataRequest", 2);
        send(authDataReq, "out", 2);
        EV << "MME prima attach request i salje Auth. data request HSS-u\n";
    }
    if (msg->getKind() == 2){
        if (msg->arrivedOn("in", 2)){
            cMessage *authReq = new cMessage("AuthenticationRequest", 2);
            send(authReq, "out", 0);
            EV << "MME prima Auth. data response od HSS-a (u kojem se nalazi XRES) te salje Auth. request UE-u (koji ukljucuje potrebne podatke za izradu RES odgovora)\n";
        }else{
            EV << "MME prima Auth. response od UE-a i usporedjuje RES i XRES. Ako su jednaki onda je autentifikacija uspjesna.\n";
            cMessage *createSessionReq = new cMessage("CreateSessionRequest", 3);
            send(createSessionReq, "out", 1);
            EV << "MME salje create session request S-GW-u\n";
        }
    }
    if (msg->getKind() == 4){
        cMessage *initialContextSetup = new cMessage("InitialContextSetup", 5);
        send(initialContextSetup, "out", 0);
        EV << "MME salje Intial context setup request poruku nadleznom eNodeB-u\n";
    }
    if (msg->getKind() == 9){
        cMessage *modifyBearer = new cMessage("ModifyBearerRequestMessage", 11);
        send(modifyBearer, "out", 1);
        EV << "MME salje modify bearer request poruku S-GW-u\n";
    }

}


