using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomicIdentifier : MonoBehaviour
{
    public bool m_Started = false;
    public LayerMask m_LayerMask;
    public float posMod = 1;
    public float radiusMod = 1;
    List<MoleculeV2> collidingMolecules;
    List<AtomV2> collidingAtoms;
    public AtomicManager manager = null;
    public GameObject nameUI;
    public GameObject dataUI;

    public bool PT = false;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("AtomManager").GetComponent<AtomicManager>();//Substituir por enciclopédia.
        collidingAtoms = new List<AtomV2>();
        collidingMolecules = new List<MoleculeV2>();
    }
    /** /
    public void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody)
        {
            if (other.attachedRigidbody.gameObject.CompareTag("Mollecule"))
            {
                //Debug.Log("hereTM");
            }
            if (other.attachedRigidbody.gameObject.CompareTag("Atom"))
            {
                //Debug.Log("hereTA");
            }
        }
    }
    /**/

    public void IdentifyMolecules()
    {
        /**/
        Vector3 botom = gameObject.transform.position - new Vector3(0, transform.localScale.y * posMod, 0);
        Vector3 top = gameObject.transform.position + new Vector3(0, transform.localScale.y * posMod, 0);
        Collider[] hitColliders = Physics.OverlapCapsule(botom, top, transform.localScale.x * 0.5f * radiusMod, m_LayerMask);
        //Check when there is a new collider coming into contact with the box
        collidingMolecules = new List<MoleculeV2>();
        collidingAtoms = new List<AtomV2>();
        foreach (Collider colider in hitColliders)//Recolhe os objetos em colisão
        {
            if (colider.attachedRigidbody)
            {
                MoleculeV2 mol;
                AtomV2 atom;
                if (colider.attachedRigidbody.CompareTag("Mollecule"))
                {

                    if (colider.attachedRigidbody.gameObject.TryGetComponent<MoleculeV2>(out mol))
                    {
                        if (!mol.isGrabed)
                        {
                            
                            if (!collidingMolecules.Contains(mol))
                            {
                                collidingMolecules.Add(mol);//Adiciona a molecula em colisão às moleculas a considerar
                            }
                            
                        }
                    }
                }
                else if (colider.attachedRigidbody.CompareTag("Atom"))
                {
                    if (colider.attachedRigidbody.gameObject.TryGetComponent<AtomV2>(out atom))
                    {
                        if (atom.molecule)
                        {
                            mol = atom.molecule;
                            if (!mol.isGrabed)
                            {
                                
                                if (!collidingMolecules.Contains(mol))
                                {
                                    collidingMolecules.Add(mol);
                                }
                               
                            }
                        }
                        else
                        {
                            if (!atom.isGrabed)
                            {
                                
                                if (!collidingAtoms.Contains(atom))
                                {
                                    collidingAtoms.Add(atom);
                                }
                                
                            }
                        }
                    }
                }
            }

        }
        //Debug.Log(collidingAtoms);
        //Debug.Log(collidingMolecules);
        if (PT)
        {
            nameUI.GetComponent<TMPro.TextMeshPro>().text = "Por favor insere uma Molécula\n->";
            dataUI.GetComponent<TMPro.TextMeshPro>().text = "Esta enciclopédia irá dar-te informação acerca das moleculas que criaste.\nColoca uma molecule no tubo e pressiona o botão.";
        }
        else
        {
            nameUI.GetComponent<TMPro.TextMeshPro>().text = "Please insert a Molecule\n->";
            dataUI.GetComponent<TMPro.TextMeshPro>().text = "This encyclopedia will give you information about the molecules you created.\nPlace a molecule in the tube and press the button.";
        }
        foreach (MoleculeV2 mol in collidingMolecules)//Aplica a ação às moléculas
        {
                //if (connection.molecule.atoms.Count == 5) Restruturar de forma a fazer isto apenas uma vez por molécula
                
            
                //{                                      
                    int hydrogenCount = 0;
                    int carbonCount = 0;
                    int oxygenCount = 0;
                    int bromineCount = 0;
                    int chlorineCount = 0;
                    int iodineCount = 0;
                    int nitrogenCount = 0;
                    int sulfurCount = 0;
                    foreach (AtomV2 atom in mol.atoms)
                    {
                        if (atom.type == AtomType.Carbon) carbonCount += 1;
                        if (atom.type == AtomType.Hydrogen) hydrogenCount += 1;
                        if (atom.type == AtomType.Oxygen) oxygenCount += 1;
                        if (atom.type == AtomType.Bromine) bromineCount += 1;
                        if (atom.type == AtomType.Chlorine) chlorineCount += 1;
                        if (atom.type == AtomType.Iodine) iodineCount += 1;
                        if (atom.type == AtomType.Nitrogen) nitrogenCount += 1;
                        if (atom.type == AtomType.Sulfur) sulfurCount += 1;
                    }
                    if (PT)
                       nameUI.GetComponent<TMPro.TextMeshPro>().text = "Molécula Não Identificada";
                    else
                        nameUI.GetComponent<TMPro.TextMeshPro>().text = "Undentified Molecule";
                    dataUI.GetComponent<TMPro.TextMeshPro>().text = "";
                    
                    if (mol.atoms.Count == 5 && carbonCount == 1 && hydrogenCount == 4)
                    {
                        //CH4
                        //Adicionar peocesso de verificação das conexões
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "CH4 - Metano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "É um hidreto do grupo 14, sendo o alcano mais simples, e o constituinte principal de gás natural. A abundância relativa de metano na Terra torna-o um combustível economicamente atrativo.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "CH4 - Methane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "It is a group-14 hydride, the simplest alkane, and the main constituent of natural gas. The relative abundance of methane on Earth makes it an economically attractive fuel.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 3 && oxygenCount == 1 && hydrogenCount == 2)
                    {
                        //H2O
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "H2O - Água";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "É uma substância química transparente, sem sabor, cheiro e quase sem cor. É o constituinte princípal da hidrosfera da Terra e dos fluidos de todos os organismos vivos.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "H2O - Water";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "It is a transparent, tasteless, odorless, and nearly colorless chemical substance, and it is the main constituent of Earth's hydrosphere and the fluids of all known living organisms.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 2 && oxygenCount == 1 && sulfurCount == 1)
                    {
                        //SO
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 2){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "SO - Monóxido de enxofre";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "É encontrado apenas como uma fase gasosa diluída. Quando concentrado ou condensado, converte-se em dióxido de dissulfureto (S2O2).";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "SO - Sulfur monoxide";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "It is only found as a dilute gas phase. When concentrated or condensed, it converts to disulfur dioxide (S2O2).";
                            }
                        }
                    }
                    if (mol.atoms.Count == 3 && oxygenCount == 2 && sulfurCount == 1)
                    {
                        //S2O
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "SO2 - Dióxido de enxofre";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "É um gás tóxico responsável pelo odor de fósforos queimados. É liberado naturalmente pela atividade vulcânica e é produzido como subproduto da extração de cobre e da queima de combustíveis fósseis contendo enxofre.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "SO2 - Sulfur dioxide";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "It is the a toxic gas responsible for the odor of burnt matches. It is released naturally by volcanic activity and is produced as a by-product of copper extraction and the burning of sulfur-bearing fossil fuels.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 4 && oxygenCount == 2 && sulfurCount == 2)
                    {
                        //S2O
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "S2O2 - Dióxido de dissulfureto";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Este sólido é instável com vida útil de alguns segundos à temperatura ambiente.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "S2O2 - Disulfur dioxide";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "This solid is unstable with a lifetime of a few seconds at room temperature.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 3 && oxygenCount == 2 && carbonCount == 1)
                    {
                        //CO2
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                    if (PT)
                    {
                        nameUI.GetComponent<TMPro.TextMeshPro>().text = "CO2 - Dióxido de Carbono";
                        dataUI.GetComponent<TMPro.TextMeshPro>().text = "É encontrado no estado gasoso à temperatura natural e, como fonte de carbono disponível no ciclo do carbono, o CO2 atmosférico é a principal fonte de carbono para a vida na Terra.";
                    }
                    else
                    {
                        nameUI.GetComponent<TMPro.TextMeshPro>().text = "CO2 - Carbon dioxide";
                        dataUI.GetComponent<TMPro.TextMeshPro>().text = "It is found in the gas state at natural temperature, and as the source of available carbon in the carbon cycle, atmospheric CO2 is the primary carbon source for life on Earth.";
                    }
                        }
                    }
                    if (mol.atoms.Count == 2 && hydrogenCount == 2)
                    {
                        //H2
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "H2 - Di-Hidrogénio";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Molécula de Hidrogénio.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "H2 - Dihydrogen";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Molecule of Hydrogen.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 2 && nitrogenCount == 2)
                    {
                        //N2
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 3){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "N2 - Azoto molécular";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Molécula de Azoto.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "N2 - Molecular nitrogen";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Molecule of Nitrogen.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 2 && bromineCount == 2)
                    {
                        //Br2
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "Br2 - Molécula de Bromo";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Molécula de Bromo.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "Br2 - Molecule of Bromine";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Molecule of Bromine.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 8 && sulfurCount == 8)
                    {
                        //S8
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "S8 - Octaenxofre";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "É um sólido amarelo inodoro e insípido e é um importante produto químico industrial. É o alótropo mais comum do enxofre e ocorre amplamente na natureza.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "S8 - Octasulfur";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "It is an odourless and tasteless yellow solid, and is a major industrial chemical. It is the most common allotrope of sulfur and occurs widely in nature.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 2 && hydrogenCount == 1 && iodineCount == 1)
                    {
                        //HI
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "HI - Iodeto de hidrogénio";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "É um gás incolor que reage com o oxigênio para dar água e iodo. Com ar humido, dá uma névoa (ou vapores) de ácido iodídrico. É excepcionalmente solúvel em água, dando ácido iodídrico.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "HI - Hydrogen iodide";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "It's is a colorless gas that reacts with oxygen to give water and iodine. With moist air, it gives a mist (or fumes) of hydroiodic acid. It is exceptionally soluble in water, giving hydroiodic acid.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 5 && carbonCount == 1 && hydrogenCount == 3 && bromineCount == 1)
                    {
                        //CH3Br
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "CH3Br - Bromometano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Este gás incolor, inodoro e não inflamável é produzido industrialmente e biologicamente. Tem uma forma tetraédrica e é um produto químico reconhecido por destruir a camada de ozono.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "CH3Br - Bromomethane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "This colorless, odorless, nonflammable gas is produced both industrially and biologically. It has a tetrahedral shape and it is a recognized ozone-depleting chemical.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 5 && carbonCount == 1 && hydrogenCount == 2 && bromineCount == 1 && chlorineCount == 1)
                    {
                        //CH2BrCl
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "CH2BrCl - Bromoclorometano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Foi inventado para uso em extintores de fogo na Alemanha em meados dos anos 40, na tentativa de criar uma alternativa menos tóxica e mais eficaz ao tetracloreto de carbono (CCl4).";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "CH2BrCl - Bromochloromethane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "It was invented for use in fire extinguishers in Germany during the mid-1940s, in an attempt to create a less toxic, more effective alternative to carbon tetrachloride (CCl4).";
                            }
                        }
                    }
                    if (mol.atoms.Count == 5 && carbonCount == 1 && hydrogenCount == 1 && bromineCount == 1 && chlorineCount == 2)
                    {
                        //BrCHCl2
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "BrCHCl2 - Bromodiclorometano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Anteriormente, foi usado como retardador de chama e solvente para gorduras e ceras e devido à sua alta densidade para separação mineral.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "BrCHCl2 - Bromodichloromethane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "It has formerly been used as a flame retardant, and a solvent for fats and waxes and because of its high density for mineral separation.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 5 && carbonCount == 1 && hydrogenCount == 1 && bromineCount == 1 && iodineCount == 2)
                    {
                        //BrCHI2
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "BrCHI2 - Bromodiiodometano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Informação Indesponível.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "BrCHI2 - Bromodiiodomethane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Unavalible Information.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 5 && carbonCount == 1 && hydrogenCount == 1 && bromineCount == 1 && iodineCount == 2)
                    {
                        //BrCHI2
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "BrCHI2 - Bromodiiodometano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Informação Indesponível.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "BrCHI2 - Bromodiiodomethane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Unavalible Information.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 5 && carbonCount == 1 && hydrogenCount == 2 && bromineCount == 1 && iodineCount == 1)
                    {
                        //BrCH2I
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "BrCH2I - Bromoiodometano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "É um líquido incolor, embora as amostras mais antigas pareçam amarelas.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "BrCH2I - Bromoiodomethane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "It is a colorless liquid, although older samples appear yellow.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 5 && carbonCount == 1 && chlorineCount == 3 && bromineCount == 1)
                    {
                        //BrCCl3
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "BrCCl3 - Bromotriclorometano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Informação Indesponível.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "BrCCl3 - Bromoiodomethane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Unavalible Information.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 5 && carbonCount == 1 && chlorineCount == 3 && bromineCount == 1)
                    {
                        //CH3Cl
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "CH3Cl - Clorometano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "É um reagente crucial na química industrial, embora raramente esteja presente em produtos de consumo, e foi anteriormente utilizado como refrigerante.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "CH3Cl - Chloromethane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "It is a crucial reagent in industrial chemistry, although it is rarely present in consumer products, and was formerly utilized as a refrigerant.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 5 && hydrogenCount == 1 && carbonCount == 1 && chlorineCount == 1 && bromineCount == 2)
                    {
                        //ClCHBr2	
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "ClCHBr2 - Clorodibromometano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "É um líquido límpido, incolor ou amarelo-laranja.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "ClCHBr2 - Chlorodibromomethane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "It is a clear colorless to yellow-orange liquid.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 5 && hydrogenCount == 1 && carbonCount == 1 && chlorineCount == 1 && iodineCount == 2)
                    {
                        //ClCHI2	
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "ClCHI2 - Clorodiiodometano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Informação Indesponível.";
                    }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "ClCHI2 - Chlorodiiodomethane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Unavalible Information.";
                    }
                        }
                    }
                    if (mol.atoms.Count == 5 && hydrogenCount == 2 && carbonCount == 1 && chlorineCount == 1 && iodineCount == 1)
                    {
                        //ClCH2I	
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "ClCH2I - Cloroiodometano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "É um líquido incolor de uso em síntese orgânica. Juntamente com outros iodometanos, o cloroiodometano é produzido por alguns microrganismos.";
                    }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "ClCH2I - Chloroiodomethane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "It is a colorless liquid of use in organic synthesis. Together with other iodomethanes, chloroiodomethane is produced by some microorganisms.";
                    }
                        }
                    }
                    if (mol.atoms.Count == 5 && carbonCount == 1 && chlorineCount == 1 && bromineCount == 3)
                    {
                        //ClCBr3	
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "ClCBr3 - Clorotribromometano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Informação Indesponível.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "ClCBr3 - Chlorotribromomethane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Unavalible Information.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 5 && carbonCount == 1 && hydrogenCount == 2 && bromineCount == 2)
                    {
                        //CH2Br2	
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "CH2Br2 - Dibromometano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "É ligeiramente solúvel em água, mas muito solúvel em solventes orgânicos. É um líquido incolor.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "CH2Br2 - Dibromomethane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "It is slightly soluble in water but very soluble in organic solvents. It is a colorless liquid.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 5 && carbonCount == 1 && bromineCount == 2 && chlorineCount == 2)
                    {
                        //Br2CCl2	
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "Br2CCl2 - Dibromodiclorometano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Informação Indesponível.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "Br2CCl2 - Dibromochloromethane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Unavalible Information.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 5 && carbonCount == 1 && bromineCount == 2 && hydrogenCount == 1 && iodineCount == 1)
                    {
                        //ICHBr2		
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "ICHBr2 - Dibromoiodometano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Informação Indesponível.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "ICHBr2 - Dibromoiodomethane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Unavalible Information.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 5 && carbonCount == 1 && hydrogenCount == 2 && chlorineCount == 2)
                    {
                        //CH2Cl2	
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "CH2Cl2 - Dicloromometano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Este líquido incolor e volátil com odor adocicado semelhante ao clorofórmio é amplamente utilizado como solvente. Embora não seja miscível com água, é ligeiramente polar e miscível com muitos solventes orgânicos.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "CH2Cl2 - Dichloromethane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "This colorless, volatile liquid with a chloroform-like, sweet odor is widely used as a solvent. Although it is not miscible with water, it is slightly polar, and miscible with many organic solvents.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 5 && carbonCount == 1 && chlorineCount == 2 && hydrogenCount == 1 && iodineCount == 1)
                    {
                        //Cl2CHI		
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "Cl2CHI - Dicloroiodometano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Informação Indesponível.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "Cl2CHI - Dichloroiodomethane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Unavalible Information.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 5 && carbonCount == 1 && hydrogenCount == 2 && iodineCount == 2)
                    {
                        //CH2I2		
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "CH2I2 - Diiodometano";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "É um líquido incolor. No entanto, ele se decompõe quando exposto à luz, liberando iodo, que dá cor às amostras em tons acastanhados. É ligeiramente solúvel em água, mas solúvel em solventes orgânicos.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "CH2I2 - Diiodomethane";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "It is a colorless liquid. However, it decomposes upon exposure to light liberating iodine, which colours samples brownish. It is slightly soluble in water, but soluble in organic solvents.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 5 && carbonCount == 1 && chlorineCount == 4)
                    {
                        //CCl4
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.size != 1){
                                v = 1;
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "CCl4 - Tetracloreto de carbono";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "É um líquido não inflamável e incolor com um cheiro “doce” semelhante ao do clorofórmio que pode ser detectado em níveis baixos. Antigamente era amplamente utilizado em extintores de fogo.";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "CCl4 - Carbon Tetrachloride";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "It is a non-flammable, colourless liquid with a “sweet” chloroform-like smell that can be detected at low levels. It was formerly widely used in fire extinguishers.";
                            }
                        }
                    }
                    if (mol.atoms.Count == 3 && oxygenCount == 1 && nitrogenCount == 2)
                    {
                        //N2O
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.A1.type==AtomType.Nitrogen && connection.A2.type == AtomType.Nitrogen)
                            {
                                if (connection.size != 2)
                                {
                                    v = 1;
                                }
                            }
                            if (connection.A1.type==AtomType.Nitrogen && connection.A2.type == AtomType.Oxygen || connection.A1.type == AtomType.Oxygen && connection.A2.type == AtomType.Nitrogen)
                            {
                                if (connection.size != 1)
                                {
                                    v = 1;
                                }
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "H2N2O2 - Ácido Hiponitroso";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Isômero de nitramida (H2N−NO2).";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "H2N2O2 - Hyponitrous acid";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Isomer of nitramide (H2N−NO2).";
                            }
                        }
                    }
                    if (mol.atoms.Count == 6 && oxygenCount == 2 && hydrogenCount == 2 && nitrogenCount == 2)
                    {
                        //H2N2O2
                        int v = 0;
                        foreach (ConnectionV2 connection in mol.connections) {
                            if (connection.A1.type==AtomType.Nitrogen && connection.A2.type == AtomType.Nitrogen)
                            {
                                if (connection.size != 2)
                                {
                                    v = 1;
                                }
                            }
                            if (connection.A1.type==AtomType.Nitrogen && connection.A2.type == AtomType.Oxygen || connection.A1.type == AtomType.Oxygen && connection.A2.type == AtomType.Hydrogen|| connection.A1.type == AtomType.Nitrogen && connection.A2.type == AtomType.Oxygen || connection.A1.type == AtomType.Oxygen && connection.A2.type == AtomType.Hydrogen)
                            {
                                if (connection.size != 1)
                                {
                                    v = 1;
                                }
                            }
                        }
                        if (v == 0){
                            if (PT) {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "H2N2O2 - Ácido Hiponitroso";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Isômero de nitramida (H2N−NO2).";
                            }
                            else {
                                nameUI.GetComponent<TMPro.TextMeshPro>().text = "H2N2O2 - Hyponitrous acid";
                                dataUI.GetComponent<TMPro.TextMeshPro>().text = "Isomer of nitramide (H2N−NO2).";
                            }
                        }
                    }
                if (mol.atoms.Count == 6 && chlorineCount == 2 && hydrogenCount == 11 && carbonCount == 5 && nitrogenCount == 1)
                {
                    int v = 0;
                    foreach (ConnectionV2 connection in mol.connections)
                    {
                            if (connection.size != 1 || connection.A1.type == AtomType.Nitrogen && connection.A2.type != AtomType.Carbon || connection.A1.type != AtomType.Carbon && connection.A2.type == AtomType.Nitrogen || connection.A1.type == AtomType.Chlorine && connection.A2.type != AtomType.Carbon || connection.A1.type != AtomType.Carbon && connection.A2.type == AtomType.Chlorine || connection.A1.type == AtomType.Hydrogen && connection.A2.type != AtomType.Carbon || connection.A1.type != AtomType.Carbon && connection.A2.type == AtomType.Hydrogen)

                            {
                                v = 1;
                            }
                    }
                    if (v == 0)
                    {
                        if (PT)
                        {
                            nameUI.GetComponent<TMPro.TextMeshPro>().text = "Mostarda nitrogenada";
                            dataUI.GetComponent<TMPro.TextMeshPro>().text = "Usada no tratamento do cancro.";
                        }
                        else
                        {
                            nameUI.GetComponent<TMPro.TextMeshPro>().text = "Nitrogen mustard";
                            dataUI.GetComponent<TMPro.TextMeshPro>().text = "Used in the treatment of cancer.";
                        }
                    }
                }
            //Debug.Log(mol);
            /*if (mol.minimized)
            {
                mol.maximize();
                manager.OnMoleculeToggle.Invoke(mol, false);
            }
            else
            {
            mol.minimize();
            manager.OnMoleculeToggle.Invoke(mol, true);*/
            //}
            /*foreach (AtomV2 atom in collidingAtoms)
            {

                //Debug.Log(atom);
                if (atom.minimized)
                {
                    atom.maximize();
                    manager.OnAtomToggle.Invoke(atom, false);

                }
                else
                {
                    atom.minimize();
                    manager.OnAtomToggle.Invoke(atom, true);
                }*/
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
        {
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Vector3 botom = gameObject.transform.position - new Vector3(0, transform.localScale.y * posMod, 0);
            Vector3 top = gameObject.transform.position + new Vector3(0, transform.localScale.y * posMod, 0);
            float radius = transform.localScale.x * 0.5f * radiusMod;
            Gizmos.DrawWireSphere(botom, radius);
            Gizmos.DrawWireSphere(top, radius);
        }
        

    }
}
